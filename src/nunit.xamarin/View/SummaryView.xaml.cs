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

using NUnit.Runner.Messages;
using NUnit.Runner.ViewModel;
using Xamarin.Forms;

namespace NUnit.Runner.View
{
    /// <summary>
    ///     The view of the test run summary.
    /// </summary>
    /// <remarks>Used as the main landing page for the test runner.</remarks>
    public partial class SummaryView : ContentPage
    {
        #region Private Fields

        /// <summary>
        ///     Holds the summary view model.
        /// </summary>
        private readonly SummaryViewModel _model;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="SummaryView"/> with the <see cref="SummaryViewModel"/>.
        /// </summary>
        /// <param name="model">The summary view model that contains the tests.</param>
        internal SummaryView(SummaryViewModel model)
        {
            _model = model;
            _model.Navigation = Navigation;
            BindingContext = _model;
            InitializeComponent();

            // Subscribe to listening for error messages
            MessagingCenter.Subscribe<ErrorMessage>(this, ErrorMessage.Name, error =>
            {
                // ReSharper disable once AsyncVoidLambda
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", error.Message, "OK"));
            });
        }

        #endregion

        #region UI Handler Methods

        /// <summary>
        /// Called when the view is appearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.OnAppearing();
        }

        #endregion
    }
}