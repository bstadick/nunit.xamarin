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
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Runner.Helpers.Filter
{
    /// <summary>
    ///     Constructs a NUnit filter.
    /// </summary>
    public class NUnitFilter
    {
        #region Private Fields

        /// <summary>
        ///     The Xml string root element tag.
        /// </summary>
        private const string _xmlTag = "filter";

        /// <summary>
        ///     The Xml string And element tag.
        /// </summary>
        private const string _xmlAndTag = "and";

        /// <summary>
        ///     The Xml string Or element tag.
        /// </summary>
        private const string _xmlOrTag = "or";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the NUnit filter Xml string.
        /// </summary>
        public string FilterXmlString { get; }

        /// <summary>
        ///     Gets the NUnit filter.
        /// </summary>
        public ITestFilter Filter
        {
            get { return TestFilter.FromXml(FilterXmlString); }
        }

        /// <summary>
        ///     Gets an empty NUnit filter.
        /// </summary>
        public static ITestFilter Empty
        {
            get { return TestFilter.FromXml(string.Empty); }
        }

        /// <summary>
        ///     Gets the root of a new NUnit filter.
        /// </summary>
        /// <remarks>Adjacent And statements are grouped and combined by interweaving Or statements to create a sum of products.</remarks>
        public static INUnitFilterElementCollection Where
        {
            get { return new NUnitFilterElementCollection(null, NUnitElementType.RootFilter); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs an NUnit filter with the given NUnit filter Xml string.
        /// </summary>
        /// <param name="filterXmlString">The NUnit filter Xml string.</param>
        private NUnitFilter(string filterXmlString)
        {
            FilterXmlString = filterXmlString;
        }

        #endregion

        #region Internal Methods

        /// <inheritdoc cref="INUnitFilterElement.Build" />
        /// <param name="leafElement">The leaf element to build the filter from.</param>
        /// <returns>The built NUnit filter.</returns>
        /// <exception cref="ArgumentNullException"><see cref="leafElement" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><see cref="leafElement" /> is not the leaf element.</exception>
        internal static NUnitFilter Build(INUnitFilterBaseElement leafElement)
        {
            if (leafElement == null)
            {
                throw ExceptionHelper.ThrowArgumentNullException(nameof(leafElement));
            }

            // Initialize elements for backwards walk from leaf to root
            INUnitFilterBaseElement current = TraverseFilterToRoot(leafElement);

            // Initialize elements for forwards walk from root to leaf where current is the root
            INUnitFilterBaseElement child = current.Child;
            bool invertNext = false;

            // Initialize parent Or collection with one And expression
            // Builder assumes sum of products (groups of And statements all combined within an Or statement, e.g. ab + bcd + e)
            ExpressionCollection<ExpressionCollection<INUnitFilterBaseElement>> orExpression =
                new ExpressionCollection<ExpressionCollection<INUnitFilterBaseElement>>(_xmlOrTag)
                {
                    new ExpressionCollection<INUnitFilterBaseElement>(_xmlAndTag)
                };

            // Walk forward from root to leaf parsing each current node along the way
            // in doing so creating the NUnit Xml filter string
            do
            {
                // Skip element if inverted as element has already been added as part of Not element
                if (invertNext)
                {
                    // Reset invert flag
                    invertNext = false;
                    // Swap child to current and get next child
                    current = child;
                    child = child?.Child;
                    // Skip processing of this iteration's current
                    continue;
                }

                switch (current.ElementType)
                {
                    case NUnitElementType.RootFilter:
                    case NUnitElementType.And:
                        // Do nothing as And/RootFilter is implicitly handled by Or collection
                        break;
                    case NUnitElementType.Or:
                        // Start new And collection
                        orExpression.Add(new ExpressionCollection<INUnitFilterBaseElement>(_xmlAndTag));
                        break;
                    case NUnitElementType.Not:
                    case NUnitElementType.Id:
                    case NUnitElementType.Test:
                    case NUnitElementType.Category:
                    case NUnitElementType.Class:
                    case NUnitElementType.Method:
                    case NUnitElementType.Namespace:
                    case NUnitElementType.Property:
                    case NUnitElementType.NUnitName:
                        // Add element to And collection
                        orExpression.Last().Add(current);
                        // If Not element, then the next child should be skipped
                        invertNext = current.ElementType == NUnitElementType.Not;
                        break;
                    default:
                        throw ExceptionHelper.ThrowArgumentOutOfRangeExceptionForElementTypeEnum(
                            nameof(current.ElementType), current.ElementType, current.ToString());
                }

                // Swap child to current and get next child
                current = child;
                child = child?.Child;
            } while (current != null);

            // Return formatted Xml string
            // If only one And element exists in the Or collection, then no Or nor And Xml tag is needed
            // as the root filter element acts as an And
            bool withXmlTag = orExpression.Count > 1 || orExpression.First().Count == 1;
            return new NUnitFilter($"<{_xmlTag}>{orExpression.ToXmlString(withXmlTag)}</{_xmlTag}>");
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Traverse the filter structure from the leaf node to the root node.
        /// </summary>
        /// <param name="leafElement">The leaf node to start with.</param>
        /// <returns>The root node.</returns>
        private static INUnitFilterBaseElement TraverseFilterToRoot(INUnitFilterBaseElement leafElement)
        {
            if (leafElement.Child != null)
            {
                throw ExceptionHelper.ThrowArgumentException(
                    "The leaf element's child is not null thus the provided leaf element is not the true leaf element. This may indicate an error in the construction or parsing of the filter.",
                    nameof(leafElement));
            }

            // Leaf doesn't have a child so start check with the child of the leaf's parent
            INUnitFilterBaseElement current = leafElement;
            INUnitFilterBaseElement parent = current.Parent;

            // Walk backwards from leaf to root
            while (parent != null)
            {
                if (parent.Child == null)
                {
                    throw ExceptionHelper.ThrowInvalidOperationExceptionForFilterBuild(
                        "The parent element's {0} child was null.", parent.ToString());
                }

                if (!ReferenceEquals(parent.Child, current))
                {
                    throw ExceptionHelper.ThrowInvalidOperationExceptionForFilterBuild(
                        "The parent element's {0} child was not the same reference as the current node.",
                        parent.ToString());
                }

                // Swap parent to current and get next current
                current = parent;
                parent = parent.Parent;
            }

            // The root is expected to be of the type RootFilter
            if (current.ElementType != NUnitElementType.RootFilter)
            {
                throw ExceptionHelper.ThrowInvalidOperationExceptionForFilterBuild(
                    "The root element type was not the expected type of RootFilter but was instead {0}.",
                    current.ElementType.ToString());
            }

            return current;
        }

        #endregion
    }
}