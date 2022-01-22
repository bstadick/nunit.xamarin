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
using NUnit.Runner.Services;
using NUnit.Runner.View;
using NUnit.Runner.ViewModel;
using Xamarin.Forms;

namespace NUnit.Runner
{
    /// <summary>
    ///     The entry point for the NUnit Xamarin test runner.
    /// </summary>
    public partial class App : Application
    {
        #region Private Fields

        /// <summary>
        ///     Holds the test runner's underlying <see cref="SummaryViewModel"/>.
        /// </summary>
        private readonly SummaryViewModel _model;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the user <see cref="TestOptions" /> for the test suite.
        /// </summary>
        public TestOptions Options
        {
            get { return _model.Options; }
            set { _model.Options = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a new <see cref="App"/>, adding the current assembly as an assembly to be tested.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Change the default background to the Windows background if running on Windows
            if (Device.RuntimePlatform == Device.UWP)
            {
                Resources["DefaultBackground"] = Resources["WindowsBackground"];
            }

            _model = new SummaryViewModel();

            // Set the main page as the summary view page
            MainPage = new NavigationPage(new SummaryView(_model));

            // Add the current calling assembly as a test assembly
            if (Options.AddCurrentAssemblyForTest)
            {
                AddTestAssembly(Assembly.GetCallingAssembly());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds an assembly to be tested to the test suite.
        /// </summary>
        /// <param name="testAssembly">The test assembly.</param>
        /// <param name="options">An optional dictionary of options for loading the assembly.</param>
        public void AddTestAssembly(Assembly testAssembly, Dictionary<string, object> options = null)
        {
            _model.AddTest(testAssembly, options);
        }

        #endregion
    }
}