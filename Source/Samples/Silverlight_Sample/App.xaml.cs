using System;
using System.Windows;

using Elmah.Everywhere;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere.Appenders;
using System.Collections.Generic;


namespace Silverlight_Sample
{
    public partial class App : Application
    {
        public App()
        {
            SetUpExceptionHandler();

            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private static void SetUpExceptionHandler()
        {
            // Configure
            var writter = new ClientHttpExceptionWritter
            {
                // NOTE: Possible to pass URI by startup arguments.
                RequestUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };

            Uri uri = Application.Current.Host.Source;
            var defaults = new ExceptionDefaults
            {
                Token = "Test-Token",
                ApplicationName = "Silverlight-Sample",
                Host = string.Format("{0}{1}{2}:{3}", uri.Scheme, Uri.SchemeDelimiter, uri.Host, uri.Port)
            };

            var appenders = new List<Type>
                       {
                           typeof (PropertiesAppender),
                           typeof (DetailAppender),
                           typeof (AssemblyAppender)
                       };


            ExceptionHandler.WithParameters(defaults, writter, appenders);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            ExceptionHandler.Report(e.ExceptionObject, null);

            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Report(ex);
            }
        }
    }
}
