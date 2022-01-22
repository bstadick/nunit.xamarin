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

using System.Threading.Tasks;
using NUnit.Runner.Helpers;

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     The base test result processor.
    /// </summary>
    internal abstract class TestResultProcessor
    {
        #region Public Properties

        /// <summary>
        ///     Gets the test suite options.
        /// </summary>
        protected TestOptions Options { get; }

        /// <summary>
        ///     Gets the next test result processor in the chain of result processing.
        /// </summary>
        protected TestResultProcessor Successor { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a base <see cref="TestResultProcessor" /> to process test results.
        /// </summary>
        /// <param name="options">The test suite options.</param>
        protected TestResultProcessor(TestOptions options)
        {
            Options = options;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Processes the test results.
        /// </summary>
        /// <param name="testResult">The test results to process.</param>
        /// <returns>A <see cref="Task" /> to await.</returns>
        public abstract Task Process(ResultSummary testResult);

        /// <summary>
        ///     Builds the chain of test processing responsibility.
        /// </summary>
        /// <param name="options">The test suite options.</param>
        /// <returns>The root test result processor in the result processing chain.</returns>
        public static TestResultProcessor BuildChainOfResponsibility(TestOptions options)
        {
            TcpWriterProcessor tcpWriter = new TcpWriterProcessor(options);
            XmlFileProcessor xmlFileWriter = new XmlFileProcessor(options);

            tcpWriter.Successor = xmlFileWriter;
            return tcpWriter;
        }

        #endregion
    }
}