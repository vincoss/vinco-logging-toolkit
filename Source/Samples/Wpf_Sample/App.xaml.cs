using System;
using System.Windows;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere;


namespace Wpf_Sample
{
    public partial class App : Application
    {
        public App()
        {
            // Configure error handler from configuration file
            ExceptionHandler.ConfigureFromConfigurationFile(new HttpExceptionWritter());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ExceptionHandler.Attach(AppDomain.CurrentDomain);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ExceptionHandler.Detach(AppDomain.CurrentDomain);
        }
    }
}