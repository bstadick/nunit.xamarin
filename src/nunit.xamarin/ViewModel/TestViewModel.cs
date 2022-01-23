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

using System.Windows.Input;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Helpers.Filter;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The individual test result summary view model.
    /// </summary>
    internal class TestViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        ///     Holds the <see cref="ITestResult"/>.
        /// </summary>
        private ITestResult _result;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the root <see cref="SummaryViewModel"/>.
        /// </summary>
        public SummaryViewModel RootModel { get; }

        /// <summary>
        ///     Gets the test result.
        /// </summary>
        public ITestResult TestResult
        {
            get { return _result; }
            private set
            {
                if (Equals(value, _result))
                {
                    return;
                }

                _result = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the command to run the tests.
        /// </summary>
        public ICommand RunTestsCommand { get; set; }

        /// <summary>
        ///     Gets the test filter corresponding to the test result.
        /// </summary>
        public ITestFilter Filter
        {
            get { return NUnitFilter.Where.Test(TestResult.Test.FullName).Build().Filter; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TestViewModel"/> with an individual <see cref="ITestResult"/>.
        /// </summary>
        /// <param name="rootModel">The root view model.</param>
        /// <param name="result">The result of an individual test.</param>
        public TestViewModel(SummaryViewModel rootModel, ITestResult result)
        {
            RootModel = rootModel;
            TestResult = result;

            // ReSharper disable once AsyncVoidLambda
            //RunTestsCommand = new Command(async o => await RootModel.ExecuteTests(Filter),
            //    o => !RootModel.Running);
        }

        #endregion
    }
}