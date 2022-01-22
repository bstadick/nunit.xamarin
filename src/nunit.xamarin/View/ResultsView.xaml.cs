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

using NUnit.Runner.ViewModel;
using Xamarin.Forms;

namespace NUnit.Runner.View
{
    /// <summary>
    ///     The view of a list of test results.
    /// </summary>
    public partial class ResultsView : ContentPage
    {
        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="ResultsView"/> with the <see cref="ResultsViewModel"/>.
        /// </summary>
        /// <param name="model">The result view model that contains the test results.</param>
        internal ResultsView(ResultsViewModel model)
        {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Handles the when a test result item is selected.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void ViewTest(object sender, SelectedItemChangedEventArgs e)
        {
            // Navigate to new test result view
            if (e.SelectedItem is ResultViewModel result)
            {
                await Navigation.PushAsync(new TestView(new TestViewModel(result.TestResult)));
            }
        }

        #endregion
    }
}