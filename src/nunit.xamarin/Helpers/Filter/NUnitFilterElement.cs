﻿// ***********************************************************************
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
    ///     Implementation of the INUnitFilterElement interface.
    /// </summary>
    /// <remarks>
    ///     Implemented by: Id, Test, Category, Class, Method, Namespace, Property, NUnitName
    /// </remarks>
    internal class NUnitFilterElement : INUnitFilterElementInternal
    {
        #region Private Fields

        /// <summary>
        ///     Holds the element's child element.
        /// </summary>
        private INUnitFilterBaseElement _child;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a new NUnit filter element with the given parent, name, and other attributes.
        /// </summary>
        /// <param name="parent">The parent of the NUnit element or <c>null</c> if the element is the root.</param>
        /// <param name="elementType">The type of NUnit filter element.</param>
        /// <param name="name">The name of the element used in the condition check.</param>
        /// <param name="isRegularExpression">If the filter is a regular expression.</param>
        /// <param name="value">The value of the element used in the condition check, if applicable otherwise is <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><see cref="parent" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        ///     <see cref="name" /> is <c>null</c> or empty
        ///     or <see cref="name" /> is <c>null</c> or empty and <see cref="elementType" /> is
        ///     <see cref="NUnitElementType.Property" />.
        /// </exception>
        public NUnitFilterElement(INUnitFilterBaseElement parent, NUnitElementType elementType, string name,
            bool isRegularExpression, string value = null)
        {
            Parent = parent ?? throw ExceptionHelper.ThrowArgumentNullException(nameof(parent));

            if (string.IsNullOrEmpty(name))
            {
                throw ExceptionHelper.ThrowArgumentExceptionForNullOrEmpty(nameof(name));
            }

            ElementName = name;

            // Element value is only needed for the Property filter
            // It can be set for other element types but will be ignored
            if (elementType == NUnitElementType.Property && string.IsNullOrEmpty(value))
            {
                throw ExceptionHelper.ThrowArgumentExceptionForNullOrEmpty(nameof(value));
            }

            ElementValue = value;

            XmlTag = MapXmlTag(elementType);
            ElementType = elementType;
            IsRegularExpression = isRegularExpression;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override string ToString()
        {
            string parentString = $"{{{Parent.GetType().Name}: {{Type: {Parent.ElementType}}}}}";
            string childString =
                Child == null ? "Null" : $"{{{Child.GetType().Name}: {{Type: {Child.ElementType}}}}}";

            return
                $"{{{GetType().Name}: {{Type: {ElementType}, Name: \"{ElementName}\", Value: \"{ElementValue}\"," +
                $" IsRegularExpression: {IsRegularExpression}, Parent: {parentString}, Child: {childString}}}}}";
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Maps the element type to the expected Xml tag string.
        /// </summary>
        /// <param name="elementType">The type of NUnit filter element.</param>
        /// <returns>The mapped type of the Xml tag string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="elementType" /> is not supported by this class.</exception>
        private static string MapXmlTag(NUnitElementType elementType)
        {
            switch (elementType)
            {
                case NUnitElementType.Id:
                    return "id";
                case NUnitElementType.Test:
                    return "test";
                case NUnitElementType.Category:
                    return "cat";
                case NUnitElementType.Class:
                    return "class";
                case NUnitElementType.Method:
                    return "method";
                case NUnitElementType.Namespace:
                    return "namespace";
                case NUnitElementType.Property:
                    return "prop";
                case NUnitElementType.NUnitName:
                    return "name";
                default:
                    throw ExceptionHelper.ThrowArgumentOutOfRangeExceptionForElementTypeEnum(nameof(elementType),
                        elementType);
            }
        }

        #endregion

        #region Implementation of INUnitFilterElement

        /// <inheritdoc />
        public INUnitFilterBaseElement Parent { get; }

        /// <inheritdoc />
        public INUnitFilterBaseElement Child
        {
            get { return _child; }
            protected set
            {
                _child =
                    value ?? throw ExceptionHelper.ThrowArgumentNullException(nameof(value));
            }
        }

        /// <inheritdoc />
        public NUnitElementType ElementType { get; }

        /// <inheritdoc />
        public string XmlTag { get; }

        /// <inheritdoc />
        public string ToXmlString(bool withXmlTag = true)
        {
            // Xml element may contain optional regular expression attribute
            string regExp = IsRegularExpression ? " re=\"1\"" : string.Empty;

            // Property type element has the name as an attribute and the value as the element content
            if (ElementType == NUnitElementType.Property)
            {
                return withXmlTag
                    ? $"<{XmlTag}{regExp} name=\"{ElementName}\">{ElementValue}</{XmlTag}>"
                    : ElementValue;
            }

            // Xml element has the name as the element content
            return withXmlTag ? $"<{XmlTag}{regExp}>{ElementName}</{XmlTag}>" : ElementName;
        }

        /// <inheritdoc />
        public string ElementName { get; }

        /// <inheritdoc />
        public string ElementValue { get; }

        /// <inheritdoc />
        public virtual bool IsRegularExpression { get; }

        /// <inheritdoc />
        public INUnitFilterElementCollection And
        {
            get
            {
                if (Child != null)
                {
                    throw ExceptionHelper.ThrowInvalidOperationExceptionForChildAlreadySet();
                }

                INUnitFilterElementCollectionInternal element =
                    new NUnitFilterElementCollection(this, NUnitElementType.And);
                Child = element;
                return element;
            }
        }

        /// <inheritdoc />
        public INUnitFilterElementCollection Or
        {
            get
            {
                if (Child != null)
                {
                    throw ExceptionHelper.ThrowInvalidOperationExceptionForChildAlreadySet();
                }

                INUnitFilterElementCollectionInternal element =
                    new NUnitFilterElementCollection(this, NUnitElementType.Or);
                Child = element;
                return element;
            }
        }

        /// <inheritdoc />
        public NUnitFilter Build()
        {
            return NUnitFilter.Build(this);
        }

        #endregion
    }
}