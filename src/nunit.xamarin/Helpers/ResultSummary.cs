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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;
using Xamarin.Forms;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Helper class used to summarize the result of a test run.
    /// </summary>
    internal class ResultSummary
    {
        #region Private Fields

        /// <summary>
        ///     Holds the test run results.
        /// </summary>
        private readonly TestRunResult _results;

        /// <summary>
        ///     Holds the cached xml results document.
        /// </summary>
        private XDocument _xmlResults;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the overall result of the test run.
        /// </summary>
        public TestStatus OverallResult { get; private set; }

        /// <summary>
        ///     Gets the color for the overall result.
        /// </summary>
        public Color OverallResultColor
        {
            get { return new ResultState(OverallResult).Color(); }
        }

        /// <summary>
        ///     Gets the number of test cases for which results have been summarized.
        ///     Any tests excluded by use of Category or Explicit attributes are not counted.
        /// </summary>
        public int TestCount { get; private set; }

        /// <summary>
        ///     Gets the number of asserts.
        /// </summary>
        public int AssertCount { get; private set; }

        /// <summary>
        ///     Gets the number of test cases actually run.
        /// </summary>
        public int RunCount
        {
            get { return PassCount + FailureCount + ErrorCount + InconclusiveCount; }
        }

        /// <summary>
        ///     Gets the number of test cases not run for any reason.
        /// </summary>
        public int NotRunCount
        {
            get { return IgnoreCount + ExplicitCount + InvalidCount + SkipCount; }
        }

        /// <summary>
        ///     Gets the count of passed tests.
        /// </summary>
        public int PassCount { get; private set; }

        /// <summary>
        ///     Gets count of failed tests, excluding errors and invalid tests.
        /// </summary>
        public int FailureCount { get; private set; }

        /// <summary>
        ///     Gets the error count.
        /// </summary>
        public int ErrorCount { get; private set; }

        /// <summary>
        ///     Gets the count of inconclusive tests.
        /// </summary>
        public int InconclusiveCount { get; private set; }

        /// <summary>
        ///     Returns the number of test cases that were not runnable due to errors in the signature of the class or method.
        ///     Such tests are also counted as Errors.
        /// </summary>
        public int InvalidCount { get; private set; }

        /// <summary>
        ///     Gets the count of skipped tests, excluding ignored and explicit tests.
        /// </summary>
        public int SkipCount { get; private set; }

        /// <summary>
        ///     Gets the ignore count.
        /// </summary>
        public int IgnoreCount { get; private set; }

        /// <summary>
        ///     Gets the count of tests not run because the are Explicit.
        /// </summary>
        public int ExplicitCount { get; private set; }

        /// <summary>
        ///     Gets the time when the test suite started running.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        ///     Gets the time when the test suite completed execution.
        /// </summary>
        public DateTime EndTime { get; }

        /// <summary>
        ///     Gets how long it took to execute the tests.
        /// </summary>
        public TimeSpan Duration
        {
            get { return EndTime.Subtract(StartTime); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="ResultSummary"/> helper to summarize the <see cref="TestRunResult"/>.
        /// </summary>
        /// <param name="results">The test run results to summarize</param>
        public ResultSummary(TestRunResult results)
        {
            _results = results;
            Initialize();
            Summarize(results.TestResults);
            StartTime = results.StartTime;
            EndTime = results.EndTime;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the collection of <see cref="ITestResult"/>.
        /// </summary>
        /// <returns>The collection of all test results.</returns>
        public IReadOnlyCollection<ITestResult> GetTestResults()
        {
            return _results.TestResults;
        }

        /// <summary>
        ///     Summarizes all of the results and returns the test result xml document.
        /// </summary>
        /// <returns>The xml test result summary document.</returns>
        public XDocument GetTestXml()
        {
            if (_xmlResults != null)
            {
                return _xmlResults;
            }

            // Add top level document element and attributes
            XElement test = new XElement("test-run");
            test.Add(new XAttribute("id", "0"));
            test.Add(new XAttribute("testcasecount", TestCount));
            test.Add(new XAttribute("total", TestCount));
            test.Add(new XAttribute("passed", PassCount));
            test.Add(new XAttribute("failed", FailureCount));
            test.Add(new XAttribute("inconclusive", InconclusiveCount));
            test.Add(new XAttribute("skipped", SkipCount));
            test.Add(new XAttribute("asserts", AssertCount));
            test.Add(new XAttribute("result", OverallResult));

            test.Add(new XAttribute("xamarin-runner-version",
                typeof(ResultSummary).GetTypeInfo().Assembly.GetName().Version.ToString()));

            DateTime startTime = _results.StartTime;
            DateTime endTime = _results.EndTime;
            double duration = endTime.Subtract(startTime).TotalSeconds;

            test.Add(new XAttribute("start-time", startTime.ToString("u")));
            test.Add(new XAttribute("end-time", endTime.ToString("u")));
            test.Add(new XAttribute("duration", duration.ToString("0.000000", NumberFormatInfo.InvariantInfo)));

            // Add each individual test result
            foreach (ITestResult result in _results.TestResults)
            {
                test.Add(XElement.Parse(result.ToXml(true).OuterXml));
            }

            // Create as xml document
            _xmlResults = new XDocument(test);
            return _xmlResults;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Initialize all properties to default values.
        /// </summary>
        private void Initialize()
        {
            TestCount = 0;
            PassCount = 0;
            FailureCount = 0;
            ErrorCount = 0;
            InconclusiveCount = 0;
            SkipCount = 0;
            IgnoreCount = 0;
            ExplicitCount = 0;
            InvalidCount = 0;
            OverallResult = TestStatus.Inconclusive;
        }

        /// <summary>
        ///     Summarize the collection of test results.
        /// </summary>
        /// <param name="results">The test results to summarize.</param>
        private void Summarize(IEnumerable<ITestResult> results)
        {
            foreach (ITestResult result in results)
            {
                Summarize(result);
            }
        }

        /// <summary>
        ///     Recursively summarize each test or test suite.
        /// </summary>
        /// <param name="result">The test or test suite to summarize.</param>
        private void Summarize(ITestResult result)
        {
            TestStatus status = TestStatus.Inconclusive;

            if (result.Test.IsSuite)
            {
                // Recursively summarize test results
                foreach (ITestResult r in result.Children)
                {
                    Summarize(r);
                }
            }
            else
            {
                // Tally up each test result
                TestCount++;
                AssertCount += result.AssertCount;

                // Handle individual test result state
                switch (result.ResultState.Status)
                {
                    case TestStatus.Passed:
                        PassCount++;
                        status = TestStatus.Passed;
                        break;
                    case TestStatus.Failed:
                        status = TestStatus.Failed;
                        if (result.ResultState == ResultState.Failure)
                        {
                            FailureCount++;
                        }
                        else if (result.ResultState == ResultState.NotRunnable)
                        {
                            InvalidCount++;
                        }
                        else
                        {
                            ErrorCount++;
                        }

                        break;
                    case TestStatus.Skipped:
                        if (result.ResultState == ResultState.Ignored)
                        {
                            IgnoreCount++;
                        }
                        else if (result.ResultState == ResultState.Explicit)
                        {
                            ExplicitCount++;
                        }
                        else
                        {
                            SkipCount++;
                        }

                        break;
                    case TestStatus.Inconclusive:
                        InconclusiveCount++;
                        break;
                }

                // Apply individual state to overall state
                switch (OverallResult)
                {
                    case TestStatus.Inconclusive:
                        OverallResult = status;
                        break;
                    case TestStatus.Passed:
                        if (status == TestStatus.Failed)
                        {
                            OverallResult = status;
                        }

                        break;
                    case TestStatus.Skipped:
                    case TestStatus.Failed:
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}