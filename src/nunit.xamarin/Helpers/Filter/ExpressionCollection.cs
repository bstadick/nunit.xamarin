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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NUnit.Runner.Helpers.Filter
{
    /// <summary>
    ///     Represents NUnit filter collection for generic serializable elements.
    /// </summary>
    internal class ExpressionCollection<T> : INUnitFilterXmlSerializableElement, ICollection<T>
        where T : INUnitFilterXmlSerializableElement
    {
        #region Private Fields

        /// <summary>
        ///     Holds the collection of elements in the collection expression.
        /// </summary>
        private readonly IList<T> _elements = new List<T>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a new NUnit filter expression collection with the given <see cref="XmlTag" /> name.
        /// </summary>
        /// <param name="xmlTag">The <see cref="XmlTag" /> to use for the collection.</param>
        /// <exception cref="ArgumentException"><see cref="xmlTag" /> is null or empty.</exception>
        public ExpressionCollection(string xmlTag)
        {
            if (string.IsNullOrEmpty(xmlTag))
            {
                throw ExceptionHelper.ThrowArgumentExceptionForNullOrEmpty(nameof(xmlTag));
            }

            XmlTag = xmlTag;
        }

        #endregion

        #region Implementation of INUnitFilterXmlSerializableElement

        /// <inheritdoc />
        public string XmlTag { get; }

        /// <inheritdoc />
        public string ToXmlString(bool withXmlTag = true)
        {
            switch (_elements.Count)
            {
                case 0:
                    // No elements so element string is empty
                    return string.Empty;
                case 1:
                    // One element so element string is just the contained element
                    return _elements.First().ToXmlString(withXmlTag);
                default:
                {
                    // Join contained elements together and optionally group inside an Xml tag
                    string elements = string.Join(string.Empty, _elements.Select(x => x.ToXmlString()));
                    return withXmlTag ? $"<{XmlTag}>{elements}</{XmlTag}>" : elements;
                }
            }
        }

        #endregion

        #region Implementation of ICollection

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            if (item == null)
            {
                return;
            }

            _elements.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _elements.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _elements.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            return _elements.Remove(item);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _elements.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}