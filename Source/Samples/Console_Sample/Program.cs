using System;
using System.Collections.Generic;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere;


namespace Console_Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Configure error handler from code
            CodeSetupExceptionHandler();

            GlobalExceptionReporting();
            ManualExceptionReporting();
        }

        private static void CodeSetupExceptionHandler()
        {
            var defaults = new ExceptionDefaults
                {
                    Token = "Test-Token",
                    ApplicationName = "Console-Sample",
                    Host = Environment.MachineName,
                    RemoteLogUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
                };

            ExceptionHandler.Configure(new HttpExceptionWritter(), defaults, null);
        }

        private static void ManualExceptionReporting()
        {
            try
            {
                int i = 0;
                int result = 10/i;
            }
            catch (Exception ex)
            {
                // Add some exception data.
                ex.Data.Add("Some-Key", "Some-Value");

                // Add some custom properties.
                var properties = CreateSampleProperties();

                // Report
                ExceptionHandler.Report(ex, properties);

                throw;
            }
        }

        private static IDictionary<string, object> CreateSampleProperties()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Test", "Value 1");
            properties.Add("Key", "Value 1");
            return properties;
        }

        private static void GlobalExceptionReporting()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler.Report((Exception)e.ExceptionObject);
        }
    }
}