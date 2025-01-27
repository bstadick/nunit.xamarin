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

using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;
using Xamarin.Forms;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The individual test result summary view model.
    /// </summary>
    internal class ResultViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets the test result.
        /// </summary>
        public ITestResult TestResult { get; }

        /// <summary>
        ///     Gets the test result status.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        ///     Gets the test name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the test parent name.
        /// </summary>
        public string Parent { get; }

        /// <summary>
        ///     Gets the test result message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the color for the result.
        /// </summary>
        public Color Color
        {
            get { return TestResult.ResultState.Color(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="ResultViewModel"/> with an individual <see cref="ITestResult"/>.
        /// </summary>
        /// <param name="result">The result of an individual test.</param>
        public ResultViewModel(ITestResult result)
        {
            TestResult = result;
            Result = StringOrNone(result.ResultState.Status.ToString().ToUpperInvariant());
            Name = StringOrNone(result.Name);
            Parent = StringOrNone(result.Test.Parent?.FullName);
            Message = StringOrNone(result.Message);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns <paramref name="str"/> or a special message if string is null or whitespace.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>The string or a special message if string is null or whitespace.</returns>
        protected static string StringOrNone(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "<none>" : str;
        }

        #endregion
    }
}