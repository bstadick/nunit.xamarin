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

namespace NUnit.Runner.Helpers.Filter
{
    /// <summary>
    ///     NUnit Xml serializable element.
    /// </summary>
    internal interface INUnitFilterXmlSerializableElement
    {
        #region Properties

        /// <summary>
        ///     Gets the Xml string element tag.
        /// </summary>
        string XmlTag { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Formats the NUnit element as its Xml string representation.
        /// </summary>
        /// <param name="withXmlTag">
        ///     <c>true</c> if the <see cref="XmlTag" /> should be included in the string, otherwise
        ///     <c>false</c> to exclude the <see cref="XmlTag" />.
        /// </param>
        /// <returns>The NUnit element Xml string representation.</returns>
        string ToXmlString(bool withXmlTag = true);

        #endregion
    }
}