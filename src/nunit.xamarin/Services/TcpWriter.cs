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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Runner.Messages;
using Xamarin.Forms;

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     Redirects stream output to a Tcp connection.
    /// </summary>
    internal class TcpWriter : TextWriter
    {
        #region Private Fields

        /// <summary>
        ///     Holds the Tcp writer connection information.
        /// </summary>
        private readonly TcpWriterInfo _info;

        /// <summary>
        ///     Holds the underlying stream writer.
        /// </summary>
        private StreamWriter _writer;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the Encoding of the stream.
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TcpWriter" /> to write stream output to a Tcp connection.
        /// </summary>
        /// <param name="info">The Tcp writer connection information.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="info" /> is null.</exception>
        public TcpWriter(TcpWriterInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            _info = info;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Opens the Tcp connection.
        /// </summary>
        /// <returns>A <see cref="Task" /> to await.</returns>
        public async Task Connect()
        {
            try
            {
                // Open the Tcp connection
                TcpClient client = new TcpClient();
                Task connect = client.ConnectAsync(_info.Hostname, _info.Port);
                Task timeout = Task.Delay(TimeSpan.FromSeconds(_info.Timeout));
                if (await Task.WhenAny(connect, timeout) == timeout)
                {
                    throw new TimeoutException();
                }

                // Get the underlying client stream
                NetworkStream stream = client.GetStream();

                // Create the stream writer to write to
                _writer = new StreamWriter(stream);
            }
            catch (TimeoutException)
            {
                MessagingCenter.Send(
                    new ErrorMessage(
                        $"Timeout connecting to {_info} after {_info.Timeout} seconds.\n\nIs your server running?"),
                    ErrorMessage.Name);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(new ErrorMessage(ex.Message), ErrorMessage.Name);
            }
        }

        #endregion

        #region Implementation of TcpWriter

        /// <inheritdoc cref="TcpWriter.Write(char)" />
        public override void Write(char value)
        {
            _writer?.Write(value);
        }

        /// <inheritdoc cref="TcpWriter.Write(string)" />
        public override void Write(string value)
        {
            _writer?.Write(value);
        }

        /// <inheritdoc cref="TcpWriter.WriteLine(string)" />
        public override void WriteLine(string value)
        {
            _writer?.WriteLine(value);
            _writer?.Flush();
        }

        /// <inheritdoc cref="TcpWriter.Dispose(bool)" />
        protected override void Dispose(bool disposing)
        {
            _writer?.Dispose();
        }

        #endregion
    }
}