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
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The test suite results view model that holds the collection of each individual test <see cref="ResultViewModel" />.
    /// </summary>
    internal class ResultsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets the collection of test results added to the view model.
        /// </summary>
        public ObservableCollection<ResultViewModel> Results { get; }

        /// <summary>
        ///     Gets if the view model shows all results, otherwise only shows those that did not pass.
        /// </summary>
        public bool IsAllResults { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="ResultsViewModel"/> with a collection of <see cref="ITestResult"/> and select filtering.
        /// </summary>
        /// <param name="results">The collection of the results in a test run.</param>
        /// <param name="viewAll">
        ///     <see langword="true" /> to add all tests, otherwise only shows those that did not pass.
        /// </param>
        public ResultsViewModel(IEnumerable<ITestResult> results, bool viewAll)
        {
            Results = new ObservableCollection<ResultViewModel>();
            IsAllResults = viewAll;
            foreach (ITestResult result in results)
            {
                AddTestResults(result, viewAll);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Recursively adds the test results to the <see cref="Results" /> collection.
        /// </summary>
        /// <param name="result">The test or test suite results to add.</param>
        /// <param name="viewAll">
        ///     <see langword="true" /> to add all tests, otherwise only shows those that did not pass.
        /// </param>
        private void AddTestResults(ITestResult result, bool viewAll)
        {
            if (result.Test.IsSuite)
            {
                // Recursively add test results
                foreach (ITestResult childResult in result.Children)
                {
                    AddTestResults(childResult, viewAll);
                }
            }
            else if (viewAll || result.ResultState.Status != TestStatus.Passed)
            {
                // Add result if all results is selected or if result is not passed
                Results.Add(new ResultViewModel(result));
            }
        }

        #endregion
    }
}