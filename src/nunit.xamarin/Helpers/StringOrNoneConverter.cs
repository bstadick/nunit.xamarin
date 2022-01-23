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
using Xamarin.Forms;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Converts a null or empty string to a default none value.
    /// </summary>
    public class StringOrNoneConverter : IValueConverter
    {
        #region Private Fields

        /// <summary>
        ///     Holds the value to use for a none string.
        /// </summary>
        private const string _noneString = "<none>";

        #endregion

        #region Implementation of IValueConverter

        /// <inheritdoc cref="Convert"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            return string.IsNullOrWhiteSpace(strValue) ? _noneString : strValue;
        }

        /// <inheritdoc cref="ConvertBack"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            return strValue == _noneString ? string.Empty : strValue;
        }

        #endregion
    }
}
