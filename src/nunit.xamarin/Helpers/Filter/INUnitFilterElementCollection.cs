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

namespace NUnit.Runner.Helpers.Filter
{
    /// <summary>
    ///     Internal NUnit filter element collection interface.
    /// </summary>
    /// <remarks>
    ///     Implemented by: Filter, And, Or
    ///     Children: Not, Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    ///     Parent: Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    /// </remarks>
    internal interface INUnitFilterElementCollectionInternal : INUnitFilterElementCollection,
        INUnitFilterContainerElementInternal
    {
    }

    /// <summary>
    ///     NUnit filter element collection interface.
    /// </summary>
    /// <remarks>
    ///     Implemented by: Filter, And, Or
    ///     Children: Not, Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    ///     Parent: Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    /// </remarks>
    public interface INUnitFilterElementCollection : INUnitFilterContainerElement
    {
        #region Properties

        /// <summary>
        ///     Gets a Not element to invert the next filter or group of filter elements.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="INUnitFilterBaseElement.Child" /> has already been set.</exception>
        INUnitFilterContainerElement Not { get; }

        #endregion
    }
}