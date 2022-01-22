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
    ///     Internal NUnit filter element interface.
    /// </summary>
    /// <remarks>
    ///     Implemented by: Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    ///     Children: And, Or
    ///     Parent: Filter, And, Or, Not
    /// </remarks>
    internal interface INUnitFilterElementInternal : INUnitFilterElement, INUnitFilterBaseElement
    {
        #region Properties

        /// <summary>
        ///     Gets the name of the element used in the condition check.
        /// </summary>
        string ElementName { get; }

        /// <summary>
        ///     Gets the value of the element used in the condition check, if applicable otherwise is <c>null</c>.
        /// </summary>
        string ElementValue { get; }

        /// <summary>
        ///     Gets if the element is to be used as a regular expression.
        /// </summary>
        bool IsRegularExpression { get; }

        #endregion
    }

    /// <summary>
    ///     NUnit filter element interface.
    /// </summary>
    /// <remarks>
    ///     Implemented by: Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    ///     Children: And, Or
    ///     Parent: Filter, And, Or, Not
    /// </remarks>
    public interface INUnitFilterElement
    {
        #region Properties

        /// <summary>
        ///     Gets an And element to group the element into.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="INUnitFilterBaseElement.Child" /> has already been set.</exception>
        INUnitFilterElementCollection And { get; }

        /// <summary>
        ///     Gets an Or element to group the element into.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="INUnitFilterBaseElement.Child" /> has already been set.</exception>
        INUnitFilterElementCollection Or { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Builds the filter from the current element chain.
        /// </summary>
        /// <remarks>Adjacent And statements are grouped and combined by interweaving Or statements to create a sum of products.</remarks>
        /// <returns>The filter built from the current element chain.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The <see cref="INUnitFilterBaseElement.ElementType" /> of an element is
        ///     not supported.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="INUnitFilterBaseElement.ElementType" /> of the root element is not
        ///     <see cref="NUnitElementType.RootFilter" />
        ///     or the <see cref="INUnitFilterBaseElement.Child" /> of an non-leaf element is <c>null</c>
        ///     or the <see cref="INUnitFilterBaseElement.Child" /> and the corresponding
        ///     <see cref="INUnitFilterBaseElement.Parent" /> are not the same referenced object.
        /// </exception>
        NUnitFilter Build();

        #endregion
    }
}