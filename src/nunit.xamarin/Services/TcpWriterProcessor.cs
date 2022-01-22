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
using System.Threading.Tasks;
using NUnit.Runner.Helpers;
using NUnit.Runner.Messages;
using Xamarin.Forms;

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     Processes the test results, writing them to a Tcp connection.
    /// </summary>
    internal class TcpWriterProcessor : TestResultProcessor
    {
        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TcpWriterProcessor" /> to redirect test results to a Tcp connection.
        /// </summary>
        /// <param name="options">The test suite options.</param>
        public TcpWriterProcessor(TestOptions options)
            : base(options) { }

        #endregion

        #region Overridden TestResultProcessor Methods

        /// <inheritdoc cref="Process" />
        public override async Task Process(ResultSummary result)
        {
            if (Options.TcpWriterParameters != null)
            {
                try
                {
                    await WriteResult(result);
                }
                catch (Exception exception)
                {
                    string message =
                        $"Fatal error while trying to send xml result by TCP to {Options.TcpWriterParameters}\n\n{exception.Message}\n\nIs your server running?";
                    MessagingCenter.Send(new ErrorMessage(message), ErrorMessage.Name);
                }
            }

            if (Successor != null)
            {
                await Successor.Process(result);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Writes the test result summary to a Tcp connection.
        /// </summary>
        /// <param name="testResult">The test result summary to write.</param>
        /// <returns>A <see cref="Task" /> to await.</returns>
        private async Task WriteResult(ResultSummary testResult)
        {
            using (TcpWriter tcpWriter = new TcpWriter(Options.TcpWriterParameters))
            {
                await tcpWriter.Connect().ConfigureAwait(false);
                tcpWriter.Write(testResult.GetTestXml());
            }
        }

        #endregion
    }
}