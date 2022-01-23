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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Runner.Helpers.Filter;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The test suite results view model that holds the collection of each individual test <see cref="TestViewModel" />.
    /// </summary>
    internal class ResultsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets the root <see cref="SummaryViewModel"/>.
        /// </summary>
        public SummaryViewModel RootModel { get; }

        /// <summary>
        ///     Gets the collection of test results added to the view model.
        /// </summary>
        public ObservableCollection<TestViewModel> Results { get; }

        /// <summary>
        ///     Gets if the view model shows all results, otherwise only shows those that did not pass.
        /// </summary>
        public bool IsAllResults { get; }

        /// <summary>
        ///     Gets or sets the command to run the tests.
        /// </summary>
        public ICommand RunTestsCommand { get; set; }

        /// <summary>
        ///     Gets the test filter corresponding to the test result.
        /// </summary>
        public ITestFilter Filter { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="ResultsViewModel"/> with a <see cref="ITestResult"/> and select filtering.
        /// </summary>
        /// <param name="rootModel">The root view model.</param>
        /// <param name="result">The result in a test run.</param>
        /// <param name="viewAll">
        ///     <see langword="true" /> to add all tests, otherwise only shows those that did not pass.
        /// </param>
        public ResultsViewModel(SummaryViewModel rootModel, ITestResult result, bool viewAll) : this(rootModel,
            new List<ITestResult> { result }, viewAll) { }

        /// <summary>
        ///     Constructs a <see cref="ResultsViewModel"/> with a collection of <see cref="ITestResult"/> and select filtering.
        /// </summary>
        /// <param name="rootModel">The root view model.</param>
        /// <param name="results">The collection of the results in a test run.</param>
        /// <param name="viewAll">
        ///     <see langword="true" /> to add all tests, otherwise only shows those that did not pass.
        /// </param>
        public ResultsViewModel(SummaryViewModel rootModel, IEnumerable<ITestResult> results, bool viewAll)
        {
            RootModel = rootModel;
            Results = new ObservableCollection<TestViewModel>();
            IsAllResults = viewAll;

            // Add test results to collection, apply selective filtering if enabled
            IList<string> testNames = AddTestResults(RootModel, results, IsAllResults, Results);

            // Set up test filter by full test name for the result view model
            Filter = CreateTestFilter(testNames);

            // Set the command to run the tests
            // ReSharper disable once AsyncVoidLambda
            //RunTestsCommand = new Command(async o => await RootModel.ExecuteTests(Filter),
            //    o => !RootModel.Running);

            // Watch for changes to the root view model properties
            RootModel.PropertyChanged += RootModelOnPropertyChanged;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Clears and adds test results from the <see cref="SummaryViewModel"/> to the provided collection.
        /// </summary>
        /// <param name="rootModel">The root view model.</param>
        /// <param name="results">The collection of the results in a test run.</param>
        /// <param name="viewAll">
        ///     <see langword="true" /> to add all tests, otherwise only shows those that did not pass.
        /// </param>
        /// <param name="filteredResults">The test results collection to add to.</param>
        /// <returns>The list of test names added to the results.</returns>
        private static IList<string> AddTestResults(SummaryViewModel rootModel, IEnumerable<ITestResult> results, bool viewAll,
            ICollection<TestViewModel> filteredResults)
        {
            // Clear previous results
            filteredResults.Clear();

            // Update with most recent results
            IList<string> testNames = new List<string>();
            foreach (ITestResult result in results)
            {
                // Skip if not filtered
                if (!viewAll && result.ResultState.Status == TestStatus.Passed)
                {
                    continue;
                }

                // Add result and test name
                filteredResults.Add(new TestViewModel(rootModel, result));
                testNames.Add(result.Test.FullName);
            }

            return testNames;
        }

        /// <summary>
        ///     Creates an inclusive <see cref="ITestFilter"/> from the list of full test names.
        /// </summary>
        /// <param name="testNames">The list of test names to filter for.</param>
        /// <returns>The created test filter.</returns>
        private static ITestFilter CreateTestFilter(IList<string> testNames)
        {
            // Set initial filters
            ITestFilter testFilter = TestFilter.Empty;
            INUnitFilterElementCollection filter = NUnitFilter.Where;

            // Add each test name to the filter
            for (int i = 0; i < testNames.Count; i++)
            {
                if (i < testNames.Count - 1)
                {
                    // More tests to be added to the filter requires to be collected by an Or statement
                    filter = filter.Test(testNames[i]).Or;
                }
                else
                {
                    // Final or only test to be added, build and assign the filter
                    testFilter = filter.Test(testNames[i]).Build().Filter;
                }
            }

            return testFilter;
        }

        /// <summary>
        ///     Handler for when the root view model properties change
        /// </summary>
        /// <param name="sender">The send object of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RootModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only listen for if the test results changes
            if (e.PropertyName != nameof(SummaryViewModel.Results))
            {
                return;
            }

            //AddTestResults(RootModel, IsAllResults, Results);
        }

        #endregion
    }
}