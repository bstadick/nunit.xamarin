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

using System;
using System.Text;
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel
{
    /// <summary>
    ///     The individual test result detailed view model.
    /// </summary>
    internal class TestViewModel : ResultViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets the test result output.
        /// </summary>
        public string Output { get; }

        /// <summary>
        ///     Gets the test results stack trace.
        /// </summary>
        /// <remarks>The StackTrace may not always be populated in the test results.</remarks>
        public string StackTrace { get; }

        /// <summary>
        ///     Gets the test properties.
        /// </summary>
        public string Properties { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TestViewModel"/> with an individual <see cref="ITestResult"/>.
        /// </summary>
        /// <param name="result">The result of an individual test.</param>
        public TestViewModel(ITestResult result) : base(result)
        {
            Output = StringOrNone(result.Output);
            StackTrace = StringOrNone(result.StackTrace);

            // Format test properties
            StringBuilder propStringBuilder = new StringBuilder();
            IPropertyBag props = result.Test.Properties;
            foreach (string key in props.Keys)
            {
                foreach (object value in props[key])
                {
                    propStringBuilder.AppendFormat("{0} = {1}{2}", key, value, Environment.NewLine);
                }
            }

            Properties = StringOrNone(propStringBuilder.ToString());
        }

        #endregion
    }
}