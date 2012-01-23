using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere;


namespace Wpf_Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetUpExceptionHandler();
        }

        private static void SetUpExceptionHandler()
        {
            // Configure
            var writter = new HttpExceptionWritter
            {
                RequestUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };

            var defaults = new ExceptionDefaults
            {
                Token = "Test-Token",
                ApplicationName = "Wpf-Sample",
                Host = Environment.MachineName
            };

            ExceptionHandler.WithParameters(defaults, writter);

            ExceptionHandler.Attach(AppDomain.CurrentDomain);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ExceptionHandler.Detach(AppDomain.CurrentDomain);
        }
    }
}