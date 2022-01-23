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
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Xamarin.Forms;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Converts a <see cref="IPropertyBag"/> to a string.
    /// </summary>

    public class TestPropertiesToStringConverter : IValueConverter
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
            if (!(value is IPropertyBag properties))
            {
                return _noneString;
            }

            // Convert property dictionary to list of key value pairs
            StringBuilder propStringBuilder = new StringBuilder();
            foreach (string key in properties.Keys)
            {
                foreach (object property in properties[key])
                {
                    propStringBuilder.AppendFormat("{0} = {1}{2}", key, property, Environment.NewLine);
                }
            }

            string result = propStringBuilder.ToString();
            return string.IsNullOrWhiteSpace(result) ? _noneString : result;
        }

        /// <inheritdoc cref="ConvertBack"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PropertyBag properties = new PropertyBag();
            string strValue = value as string;
            if (string.IsNullOrWhiteSpace(strValue) || strValue == _noneString)
            {
                return properties;
            }

            // Convert list of key value pairs to property dictionary
            string[] lines = strValue.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                int delimiterInd = line.IndexOf('=');
                if (delimiterInd == -1)
                {
                    continue;
                }

                string propKey = line.Substring(0, delimiterInd).Trim();
                string propValue = line.Substring(delimiterInd + 1).Trim();

                properties.Add(propKey, propValue);
            }

            return properties;
        }

        #endregion
    }
}
