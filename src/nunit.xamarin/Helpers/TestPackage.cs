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

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Runner.Helpers
{
    /// <summary>
    ///     Contains all assemblies for a test run, and controls execution of tests and collection of results.
    /// </summary>
    internal class TestPackage
    {
        #region Private Fields

        /// <summary>
        ///     Holds the list of test assemblies in the test suite.
        /// </summary>
        private readonly List<(Assembly, Dictionary<string, object>)> _testAssemblies =
            new List<(Assembly, Dictionary<string, object>)>();

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds an assembly to be tested to the test suite.
        /// </summary>
        /// <param name="testAssembly">The test assembly.</param>
        /// <param name="options">An optional dictionary of options for loading the assembly.</param>
        public void AddAssembly(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            _testAssemblies.Add((testAssembly, options));
        }

        /// <summary>
        ///     Executes the test assemblies.
        /// </summary>
        /// <returns>The test run results.</returns>
        public async Task<TestRunResult> ExecuteTests()
        {
            TestRunResult resultPackage = new TestRunResult();

            foreach ((Assembly assembly, Dictionary<string, object> options) in _testAssemblies)
            {
                NUnitTestAssemblyRunner runner = await LoadTestAssemblyAsync(assembly, options).ConfigureAwait(false);
                ITestResult result = await Task.Run(() => runner.Run(TestListener.NULL, TestFilter.Empty))
                                               .ConfigureAwait(false);
                resultPackage.AddResult(result);
            }

            resultPackage.CompleteTestRun();
            return resultPackage;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Loads the test assembly to be ran.
        /// </summary>
        /// <param name="assembly">The test assembly.</param>
        /// <param name="options">An optional dictionary of options for loading the assembly.</param>
        /// <returns></returns>
        private static async Task<NUnitTestAssemblyRunner> LoadTestAssemblyAsync(Assembly assembly,
            Dictionary<string, object> options)
        {
            NUnitTestAssemblyRunner runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            await Task.Run(() => runner.Load(assembly, options ?? new Dictionary<string, object>()));
            return runner;
        }

        #endregion
    }
}