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
    ///     The type of the NUnit filter element.
    /// </summary>
    /// <see href="https://github.com/nunit/docs/wiki/Test-Filters" />
    public enum NUnitElementType
    {
        /// <summary>
        ///     The root filter element. Acts as an And filter.
        /// </summary>
        /// <remarks>Xml tag is "filter"</remarks>
        RootFilter,

        /// <summary>
        ///     An And filter element that combines multiple filter elements together using "and" constructs.
        /// </summary>
        /// <remarks>Xml tag is "and"</remarks>
        And,

        /// <summary>
        ///     An Or filter element that combines multiple filter elements together using "or" constructs.
        /// </summary>
        /// <remarks>Xml tag is "or"</remarks>
        Or,

        /// <summary>
        ///     A Not filter element that inverts the condition of the containing filter element.
        /// </summary>
        /// <remarks>Xml tag is "not"</remarks>
        Not,

        /// <summary>
        ///     An Id filter element that filters based on the NUnit given test id.
        /// </summary>
        /// <remarks>Xml tag is "id"</remarks>
        Id,

        /// <summary>
        ///     A Test name filter element that filters based on the full name of the test.
        /// </summary>
        /// <remarks>Xml tag is "test"</remarks>
        Test,

        /// <summary>
        ///     A Category filter element that filters on the category attribute of the test.
        /// </summary>
        /// <remarks>Xml tag is "cat"</remarks>
        Category,

        /// <summary>
        ///     A Class filter element that filters on the full class name of the test.
        /// </summary>
        /// <remarks>Xml tag is "class"</remarks>
        Class,

        /// <summary>
        ///     A Method filter element that filters on the full method name of the test.
        /// </summary>
        /// <remarks>Xml tag is "method"</remarks>
        Method,

        /// <summary>
        ///     A Namespace filter element that filters on the full namespace of the test.
        /// </summary>
        /// <remarks>Xml tag is "namespace"</remarks>
        Namespace,

        /// <summary>
        ///     A Property filter element that filters on the property attribute name and value of the test.
        /// </summary>
        /// <remarks>Xml tag is "prop"</remarks>
        Property,

        /// <summary>
        ///     A Name filter element that filters on the NUnit given test name.
        /// </summary>
        /// <remarks>Xml tag is "name"</remarks>
        NUnitName
    }
}