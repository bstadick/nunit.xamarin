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

using System.IO;
using Foundation;
using NUnit.Runner.Services;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace NUnit.Runner.Tests
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once RedundantNameQualifier
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // ReSharper disable once RedundantNameQualifier
            global::Xamarin.Forms.Forms.Init();

            // This will load all tests within the current project
            App nunit = new App();

            // If you want to add tests in another assembly
            //nunit.AddTestAssembly(typeof(MyTests).Assembly);
            // Or, if you want to add tests with an extra test options dictionary
            //nunit.AddTestAssembly(typeof(MyTests).Assembly, new Dictionary<string, object>());

            // Available options for testing
            nunit.Options = new TestOptions
            {
                // If True, the tests will run automatically when the app starts
                // otherwise you must run them manually.
                AutoRun = true,

                // If True, adds the current calling assembly to the test assembly list.
                //AddCurrentAssemblyForTest = true,

                // Set a progress listener to update progress on each test ran.
                //ProgressListener = new NUnit.Framework.Internal.TestProgressReporter(null),

                // Information about the tcp listener host and port.
                // For now, send result as XML to the listening server.
                //TcpWriterParameters = new TcpWriterInfo("192.168.0.108", 13000),

                // Creates a NUnit Xml result file on the host file system using PCLStorage library.
                CreateXmlResultFile = true,

                // Choose a different path for the xml result file (ios file share / library directory)
                ResultFilePath =
                    Path.Combine(
                        NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory,
                            NSSearchPathDomain.User)[0].Path, "Results.xml")
            };

            // Sets the command to execute after running the tests such as custom handling of the results or exiting the application.
            //nunit.Options.OnTestRunCompleted += (testResults) =>
            //{
            //    ObjCRuntime.Selector selector = new ObjCRuntime.Selector("terminateWithSuccess");
            //    UIKit.UIApplication.SharedApplication.PerformSelector(selector, UIKit.UIApplication.SharedApplication,
            //        0);
            //};

            LoadApplication(nunit);

            return base.FinishedLaunching(app, options);
        }
    }
}