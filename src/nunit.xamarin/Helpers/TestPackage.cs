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
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Runner.Annotations;
using NUnit.Runner.Services;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Contains all assemblies for a test run, and controls execution of tests and collection of results.
    /// </summary>
    internal class TestPackage : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        ///     Holds the list of test assemblies in the test suite.
        /// </summary>
        private readonly List<(Assembly, Dictionary<string, object>)> _testAssemblies =
            new List<(Assembly, Dictionary<string, object>)>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the user options for the test suite.
        /// </summary>
        public TestOptions Options { get; set; }

        /// <summary>
        ///     Gets the progress listener for running tests.
        /// </summary>
        public RunnerTestListener ProgressListener { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TestPackage"/>.
        /// </summary>
        /// <param name="options">The test options for the test package.</param>
        public TestPackage(TestOptions options)
        {
            Options = options;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds an assembly to be tested to the test suite.
        /// </summary>
        /// <param name="testAssembly">The test assembly.</param>
        /// <param name="options">An optional dictionary of options for loading the assembly.</param>
        public void AddAssembly(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            _testAssemblies.Add((testAssembly, options));
        }

        /// <summary>
        ///     Executes the test assemblies.
        /// </summary>
        /// <param name="filter">The xml test filter of tests to run.</param>
        /// <returns>The test run results.</returns>
        public async Task<TestRunResult> ExecuteTests(ITestFilter filter = null)
        {
            ITestFilter testFilter = filter ?? TestFilter.Empty;
            TestRunResult resultPackage = new TestRunResult();

            // Initialize test runner for each test assembly
            NUnitTestAssemblyRunner runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            foreach ((Assembly assembly, Dictionary<string, object> options) in _testAssemblies)
            {
                await LoadTestAssemblyAsync(runner, assembly, options).ConfigureAwait(false);
            }

            // Configure the progress listener
            long testCount = runner.CountTestCases(testFilter);
            ProgressListener = new RunnerTestListener(Options?.ProgressListener, testCount);
            ProgressListener.PropertyChanged += ProgressListenerOnPropertyChanged;

            // Execute test runners
            ITestResult result = await Task.Run(() => runner.Run(ProgressListener, testFilter))
                                           .ConfigureAwait(false);
            resultPackage.AddResult(result);

            // Return the results
            resultPackage.CompleteTestRun();
            return resultPackage;
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Calls the <see cref="PropertyChanged" /> event for the given property.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Loads the test assembly to be ran.
        /// </summary>
        /// <param name="runner">The unit test runner.</param>
        /// <param name="assembly">The test assembly.</param>
        /// <param name="options">An optional dictionary of options for loading the assembly.</param>
        /// <returns>A <see cref="Task"/> to await.</returns>
        private static async Task LoadTestAssemblyAsync(NUnitTestAssemblyRunner runner,
            Assembly assembly,
            Dictionary<string, object> options)
        {
            await Task.Run(() => runner.Load(assembly, options ?? new Dictionary<string, object>()));
        }

        /// <summary>
        ///     Handler for when the <see cref="ProgressListener"/> properties have changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ProgressListenerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Notify that the underlying ProgressListener property has updated
            OnPropertyChanged(nameof(ProgressListener));
        }

        #endregion
    }
}