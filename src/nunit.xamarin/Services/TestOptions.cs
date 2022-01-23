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
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Helpers;

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     Options for the test runner test suite.
    /// </summary>
    public class TestOptions
    {
        #region Private Fields

        /// <summary>
        ///     Holds the test output xml report name.
        /// </summary>
        private const string _outputXmlReportName = "TestResults.xml";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets if the tests will run automatically when the app starts, otherwise tests to be ran manually.
        /// </summary>
        public bool AutoRun { get; set; }

        /// <summary>
        ///     Gets or sets if to add the current calling assembly to the test assembly list.
        /// </summary>
        public bool AddCurrentAssemblyForTest { get; set; }

        /// <summary>
        ///     Gets or sets the test progress listener.
        /// </summary>
        public ITestListener ProgressListener { get; set; }

        /// <summary>
        ///     Gets information about the tcp listener host and port.
        /// </summary>
        /// <remarks>For now, send result as XML to the listening server.</remarks>
        public TcpWriterInfo TcpWriterParameters { get; set; }

        /// <summary>
        ///     Gets or sets if to create a NUnit Xml result file on the host file system.
        /// </summary>
        public bool CreateXmlResultFile { get; set; }

        /// <summary>
        ///     Gets or sets the file path for the xml result file. Default is [LocalStorage]/TestResults.xml
        /// </summary>
        public string ResultFilePath { get; set; }

        #endregion

        #region Events

        /// <summary>
        ///     Event fired when the test run has completed.
        /// </summary>
        public event EventHandler<TestRunCompletedEventArgs> OnTestRunCompleted;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TestOptions" />.
        /// </summary>
        public TestOptions()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ResultFilePath = Path.Combine(path, _outputXmlReportName);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///     Invokes <see cref="OnTestRunCompleted"/> when the test run has completed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="testResults">The results of the test run.</param>
        internal async Task InvokeOnTestRunCompleted(object sender, ResultSummary testResults)
        {
            if (OnTestRunCompleted != null)
            {
                await Task.Run(() =>
                    OnTestRunCompleted?.Invoke(sender, new TestRunCompletedEventArgs { TestResults = testResults }));
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        #endregion
    }

    /// <summary>
    ///     Event arguments for the <see cref="TestOptions.OnTestRunCompleted"/> event.
    /// </summary>
    public class TestRunCompletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets or sets the test result summary.
        /// </summary>
        public ResultSummary TestResults { get; set; }
    }
}