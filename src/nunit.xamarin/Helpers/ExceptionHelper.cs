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
using System.Globalization;
using System.Text.RegularExpressions;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Helper class for throwing common exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        #region Public Methods

        /// <summary>
        ///     Creates a new <see cref="ArgumentNullException" /> to throw when an object is null.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was null.</param>
        /// <returns>A new <see cref="ArgumentNullException" /> to throw.</returns>
        public static ArgumentNullException ThrowArgumentNullException(string paramName)
        {
            return new ArgumentNullException(paramName, Format("The {0} cannot be null.", paramName));
        }

        /// <summary>
        ///     Creates a new <see cref="ArgumentException" /> to throw when an object is null or empty.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was null.</param>
        /// <returns>A new <see cref="ArgumentException" /> to throw.</returns>
        public static ArgumentException ThrowArgumentExceptionForNullOrEmpty(string paramName)
        {
            return new ArgumentException(Format("The {0} cannot be null or empty.", paramName),
                paramName);
        }

        /// <summary>
        ///     Creates a new <see cref="ArgumentException" /> to throw when an argument is invalid.
        /// </summary>
        /// <param name="msg">The message of the exception.</param>
        /// <param name="paramName">The name of the parameter that was null.</param>
        /// <returns>A new <see cref="ArgumentException" /> to throw.</returns>
        public static ArgumentException ThrowArgumentException(string msg, string paramName)
        {
            return new ArgumentException(msg, paramName);
        }

        /// <summary>
        ///     Creates a new <see cref="ArgumentOutOfRangeException" /> to throw when the <see cref="Filter.NUnitElementType" /> enum is
        ///     out of range.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was out of range.</param>
        /// <param name="actualValue">The value that was out of range.</param>
        /// <param name="arg">An optional message format argument.</param>
        /// <returns>A new <see cref="ArgumentOutOfRangeException" /> to throw.</returns>
        public static ArgumentOutOfRangeException ThrowArgumentOutOfRangeExceptionForElementTypeEnum(string paramName,
            object actualValue, string arg = "")
        {
            // Add a space if arg is not empty and does not already start with whitespace
            if (!string.IsNullOrEmpty(arg) && !Regex.IsMatch(arg, "^\\s"))
            {
                arg = " " + arg;
            }

            return new ArgumentOutOfRangeException(paramName, actualValue, Format(
                "The given element type is not supported.{0}", arg));
        }

        /// <summary>
        ///     Creates a new <see cref="ArgumentOutOfRangeException" /> to throw when a value is less than zero.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was out of range.</param>
        /// <param name="actualValue">The value that was out of range.</param>
        /// <returns>A new <see cref="ArgumentOutOfRangeException" /> to throw.</returns>
        public static ArgumentOutOfRangeException ThrowArgumentOutOfRangeExceptionForValueLessThanZero(string paramName,
            object actualValue)
        {
            return new ArgumentOutOfRangeException(paramName, actualValue,
                Format("The {0} must be greater than or equal to 0.", paramName));
        }

        /// <summary>
        ///     Creates a new <see cref="InvalidOperationException" /> to throw when the child element has already been set.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException" /> to throw.</returns>
        public static InvalidOperationException ThrowInvalidOperationExceptionForChildAlreadySet()
        {
            return new InvalidOperationException("The child element has already been set for this instance.");
        }

        /// <summary>
        ///     Creates a new <see cref="InvalidOperationException" /> to throw when the <see cref="Filter.NUnitFilter.Build" /> traversal
        ///     fails.
        /// </summary>
        /// <param name="msg">The message of the exception.</param>
        /// <param name="parent">The parent node of the exception.</param>
        /// <returns>A new <see cref="InvalidOperationException" /> to throw.</returns>
        public static InvalidOperationException ThrowInvalidOperationExceptionForFilterBuild(string msg,
            string parent)
        {
            return new InvalidOperationException(Format(msg, parent) +
                                                 " Forward traversal will not proceed properly. This may indicate an error in the construction or parsing of the filter.");
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Replaces the format item in a specified string with the string representation of a corresponding object in a
        ///     specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        ///     A copy of <see cref="format" /> in which the format items have been replaced by the string representation of
        ///     the corresponding objects in <see cref="args" />.
        /// </returns>
        private static string Format(string format, params object[] args)
        {
            format = format ?? string.Empty;
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        #endregion
    }
}