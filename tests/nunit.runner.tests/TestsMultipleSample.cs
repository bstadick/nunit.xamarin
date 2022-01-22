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
using NUnit.Framework;

namespace NUnit.Runner.Tests
{
    [TestFixture]
    [Author("Rob Prouse")]
    public class TestsMultipleSample
    {
        [SetUp]
        public void Setup() { }


        [TearDown]
        public void Tear() { }

        [Test]
        [Category("Passing")]
        public void PassMultiple()
        {
            Assert.Multiple(() =>
            {
                TestContext.WriteLine("Capture some output");
                Assert.True(true);
                Assert.True(true);
            });
        }

        [Test]
        [Category("Failing")]
        [Author("Code Monkey")]
        public void FailMultiple()
        {
            Assert.Multiple(() =>
            {
                Assert.True(true);
                Assert.True(false);
                Assert.True(true);
                Assert.True(false);
            });
        }

        [Test]
        [Ignore("another time")]
        public void IgnoreMultiple()
        {
            Assert.Multiple(() =>
            {
                Assert.True(true);
                Assert.True(false);
                Assert.True(true);
                Assert.True(false);
            });
        }

        [Test]
        [Explicit("This is only run on demand")]
        public void ExplicitMultiple()
        {
            Assert.Multiple(() =>
            {
                Assert.True(true);
                Assert.True(true);
            });
        }

        // Note that Assert.Pass, Assert.Ignore, Assert.Inconclusive, and Assume.That may not be used inside a multiple assert block

        [Test]
        public void ErrorMultiple()
        {
            Assert.Multiple(() =>
            {
                Assert.True(true);
                TestContext.WriteLine("I am about to throw!!!");
                throw new NotSupportedException("This method isn't ready yet");
            });
        }

        [Test]
        public void WarningMultiple()
        {
            Assert.Multiple(() =>
            {
                Assert.True(true);
                Assert.Warn("Warning");
                Assert.Warn("Another Warning");
            });
        }
    }
}
