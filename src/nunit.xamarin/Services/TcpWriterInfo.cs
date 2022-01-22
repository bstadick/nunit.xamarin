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

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     Represents the Tcp host and port to connect to.
    /// </summary>
    public class TcpWriterInfo : IEquatable<TcpWriterInfo>
    {
        #region Public Properties

        /// <summary>
        ///     Gets the host to connect to.
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        ///     Gets the port to connect to.
        /// </summary>
        public int Port { get; }

        /// <summary>
        ///     Gets the connection timeout in seconds.
        /// </summary>
        public int Timeout { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="TcpWriterInfo" /> with the host name, port number, and connection timeout.
        /// </summary>
        /// <param name="hostName">The host name or IP to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="timeout">The timeout in seconds.</param>
        public TcpWriterInfo(string hostName, int port, int timeout = 10)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if ((port <= 0) || (port > ushort.MaxValue))
            {
                throw new ArgumentException("Must be between 1 and ushort.MaxValue", nameof(port));
            }

            if (timeout <= 0)
            {
                throw new ArgumentException("Must be positive", nameof(timeout));
            }

            Hostname = hostName;
            Port = port;
            Timeout = timeout;
        }

        #endregion

        #region Overridden Object Methods

        /// <inheritdoc cref="Equals(object)" />
        public override bool Equals(object obj)
        {
            return Equals(obj as TcpWriterInfo);
        }

        /// <summary>
        ///     Compares whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter, otherwise
        ///     <see langword="false" />.
        /// </returns>
        public bool Equals(TcpWriterInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Hostname == other.Hostname && Port == other.Port && Timeout == other.Timeout;
        }

        /// <inheritdoc cref="GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Hostname != null ? Hostname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Port;
                hashCode = (hashCode * 397) ^ Timeout;
                return hashCode;
            }
        }

        /// <summary>
        ///     Compares whether one object is equal to another object of the same type.
        /// </summary>
        /// <param name="left">The left side object to compare.</param>
        /// <param name="right">The right side object to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the <paramref name="left"/> parameter is equal to the <paramref name="right" /> parameter,
        ///     otherwise <see langword="false" />.
        /// </returns>
        public static bool operator ==(TcpWriterInfo left, TcpWriterInfo right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Compares whether one object is not equal to another object of the same type.
        /// </summary>
        /// <param name="left">The left side object to compare.</param>
        /// <param name="right">The right side object to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if the <paramref name="left"/> parameter is not equal to the <paramref name="right" /> parameter,
        ///     otherwise <see langword="false" />.
        /// </returns>
        public static bool operator !=(TcpWriterInfo left, TcpWriterInfo right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc cref="ToString" />
        public override string ToString()
        {
            return $"{Hostname}:{Port}";
        }

        #endregion
    }
}