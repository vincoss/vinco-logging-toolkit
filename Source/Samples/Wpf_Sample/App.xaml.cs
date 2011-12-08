using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Elmah.Everywhere.Handlers;
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
            HttpHandler handler = new HttpHandler
            {
                RequestUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };
            ExceptionHandler.SetWritter(new HttpExceptionWritter(handler));
            ExceptionHandler.SetParameters(new ExceptionParameters
            {
                Token = null,
                ApplicationName = "Exceptions-Handler",
                Host = "Wpf-Client"
            });

            ExceptionHandler.Attach(AppDomain.CurrentDomain);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ExceptionHandler.Detach(AppDomain.CurrentDomain);
        }
    }
}
