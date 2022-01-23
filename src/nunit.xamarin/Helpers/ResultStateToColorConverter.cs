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
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;
using Xamarin.Forms;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Converts a <see cref="ResultState"/> to a mapped result color.
    /// </summary>
    public class ResultStateToColorConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc cref="Convert"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResultState state = value as ResultState;
            return state?.Color() ?? Color.Default;
        }

        /// <inheritdoc cref="ConvertBack"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Converting back not supported
            return Color.Gray;
        }

        #endregion
    }
}
