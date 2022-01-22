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
    ///     Base NUnit filter element interface.
    /// </summary>
    /// <see href="http://github.com/nunit/docs/wiki/Test-Filters" />
    internal interface INUnitFilterBaseElement : INUnitFilterXmlSerializableElement
    {
        #region Properties

        /// <summary>
        ///     Gets the parent of the NUnit element or <c>null</c> if the element is the root.
        /// </summary>
        INUnitFilterBaseElement Parent { get; }

        /// <summary>
        ///     Gets the child of the NUnit element or <c>null</c> if the element is the leaf.
        /// </summary>
        INUnitFilterBaseElement Child { get; }

        /// <summary>
        ///     Gets the type of the NUnit element.
        /// </summary>
        NUnitElementType ElementType { get; }

        #endregion
    }
}