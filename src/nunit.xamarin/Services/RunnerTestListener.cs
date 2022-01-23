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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Annotations;

namespace NUnit.Runner.Services
{
    /// <summary>
    ///     Creates a custom <see cref="ITestListener"/> to track progress internally.
    /// </summary>
    internal class RunnerTestListener : ITestListener, INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        ///     Holds the user test listener.
        /// </summary>
        private readonly ITestListener _listener;

        /// <summary>
        ///     Holds the number of tests ran.
        /// </summary>
        private long _ranCount;

        /// <summary>
        ///     Holds the currently running test.
        /// </summary>
        private string _currentTest;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the test progress as a percentage between [0, 1].
        /// </summary>
        public double Progress
        {
            get { return TestCount != 0 ? RanCount / (double)TestCount : 0; }
        }

        /// <summary>
        ///     Gets the number of tests to run.
        /// </summary>
        public long TestCount { get; }

        /// <summary>
        ///     Gets the number of tests ran.
        /// </summary>
        public long RanCount
        {
            get { return _ranCount; }
            private set
            {
                _ranCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progress));
            }
        }

        /// <summary>
        ///     Gets the currently running test name.
        /// </summary>
        public string CurrentTest
        {
            get { return string.IsNullOrWhiteSpace(_currentTest) ? string.Empty : _currentTest; }
            private set
            {
                _currentTest = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a <see cref="RunnerTestListener"/> to track test progress.
        /// </summary>
        /// <param name="listener">The user provided test listener to invoke.</param>
        /// <param name="testCount">The number of tests being ran.</param>
        public RunnerTestListener(ITestListener listener, long testCount)
        {
            _listener = listener;
            TestCount = testCount;
        }

        #endregion

        #region Implementation of ITestListener

        /// <inheritdoc cref="TestStarted"/>
        public void TestStarted(ITest test)
        {
            // Update currently running test
            if (!test.HasChildren)
            {
                CurrentTest = test.FullName;
            }

            _listener?.TestStarted(test);
        }

        /// <inheritdoc cref="TestFinished"/>
        public void TestFinished(ITestResult result)
        {
            // Update test progress
            if (!result.HasChildren)
            {
                RanCount++;
                CurrentTest = string.Empty;
            }

            _listener?.TestFinished(result);
        }

        /// <inheritdoc cref="TestOutput"/>
        public void TestOutput(TestOutput output)
        {
            _listener?.TestOutput(output);
        }

        /// <inheritdoc cref="SendMessage"/>
        public void SendMessage(TestMessage message)
        {
            _listener?.SendMessage(message);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Calls the <see cref="PropertyChanged" /> event for the given property.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
