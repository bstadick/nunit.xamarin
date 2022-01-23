// ***********************************************************************
// Copyright (c) 2022 NUnit Project
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Helpers;
using NUnit.Runner.Services;
using NUnit.Runner.View;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The summary view of the test suite.
    /// </summary>
    internal class SummaryViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        ///     Holds the user test options.
        /// </summary>
        private TestOptions _options;

        /// <summary>
        ///     Holds the test result processor.
        /// </summary>
        private TestResultProcessor _resultProcessor;

        /// <summary>
        ///     Holds the test result summary.
        /// </summary>
        private ResultSummary _results;

        /// <summary>
        ///     Holds if the test suite is running.
        /// </summary>
        private bool _running;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the <see cref="TestPackage"/> used to run the tests.
        /// </summary>
        public TestPackage Package { get; }

        /// <summary>
        ///     Gets or sets the user options for the test suite.
        /// </summary>
        public TestOptions Options
        {
            get { return _options ?? (_options = new TestOptions()); }
            set { _options = value ?? new TestOptions(); }
        }

        /// <summary>
        ///     Gets or sets the overall test results summary.
        /// </summary>
        public ResultSummary Results
        {
            get { return _results; }
            private set
            {
                if (Equals(value, _results))
                {
                    return;
                }

                _results = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasResults));
            }
        }

        /// <summary>
        ///     Gets or sets if tests are currently running.
        /// </summary>
        public bool Running
        {
            get { return _running; }
            private set
            {
                if (value.Equals(_running))
                {
                    return;
                }

                _running = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets if there are test results to display.
        /// </summary>
        public bool HasResults
        {
            get { return Results != null; }
        }

        /// <summary>
        ///     Gets or sets the command to run the tests.
        /// </summary>
        public ICommand RunTestsCommand { get; set; }

        /// <summary>
        ///     Gets or sets the command to view all test results.
        /// </summary>
        public ICommand ViewAllResultsCommand { get; set; }

        /// <summary>
        ///     Gets or sets the command to view all failed test results.
        /// </summary>
        public ICommand ViewFailedResultsCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="SummaryViewModel"/> for the test runner.
        /// </summary>
        public SummaryViewModel()
        {
            Package = new TestPackage();

            // ReSharper disable AsyncVoidLambda

            // Set the command to run the tests
            RunTestsCommand = new Command(async o => await ExecuteTests(), o => !Running);

            // Set the commands to display the results
            ViewAllResultsCommand = new Command(
                async o => await Navigation.PushAsync(
                    new ResultsView(new ResultsViewModel(this, Results.GetTestResults(), true))),
                o => !HasResults);
            ViewFailedResultsCommand = new Command(
                async o => await Navigation.PushAsync(
                    new ResultsView(new ResultsViewModel(this, Results.GetTestResults(), false))),
                o => !HasResults);

            // ReSharper restore AsyncVoidLambda
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///     Called from the view when the view is appearing.
        /// </summary>
        internal void OnAppearing()
        {
            // Check if to auto run tests
            if (!Options.AutoRun)
            {
                return;
            }

            // Set to not rerun if navigated back
            Options.AutoRun = false;

            // Auto run tests
            RunTestsCommand.Execute(null);
        }

        /// <summary>
        ///     Adds an assembly to be tested.
        /// </summary>
        /// <param name="testAssembly">The test assembly.</param>
        /// <param name="options">The test options.</param>
        internal void AddTest(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            Package.AddAssembly(testAssembly, options);
        }

        /// <summary>
        ///     Executes the tests.
        /// </summary>
        /// <returns>A <see cref="Task" /> to await.</returns>
        internal async Task ExecuteTests(ITestFilter filter = null)
        {
            Running = true;
            Results = null;

            // Run tests
            TestRunResult results = await Package.ExecuteTests(filter);

            // Process results
            ResultSummary summary = new ResultSummary(results);
            _resultProcessor = TestResultProcessor.BuildChainOfResponsibility(Options);
            await _resultProcessor.Process(summary).ConfigureAwait(false);

            // Report results on main thread as setting these properties will invoke binding updates
            Device.BeginInvokeOnMainThread(
                // ReSharper disable once AsyncVoidLambda
                async () =>
                {
                    Results = summary;
                    Running = false;

                    await Options.InvokeOnTestRunCompleted(summary);
                });
        }

        #endregion
    }
}