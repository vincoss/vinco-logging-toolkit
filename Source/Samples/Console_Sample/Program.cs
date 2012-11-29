using System;
using System.Collections.Generic;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere;


namespace Console_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure error handler from code
            CodeSetupExceptionHandler();
            
            // Create sample exception and properties
            Exception exception = CreateSampleException();
            IDictionary<string, object> properties = CreateSampleProperties();

            // Manual report exception
            ExceptionHandler.Report(exception, properties);

            Console.WriteLine("Done...");
            Console.Read();
        }

        private static void CodeSetupExceptionHandler()
        {
            var writter = new HttpExceptionWritter
            {
                RequestUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };

            var defaults = new ExceptionDefaults
            {
                Token = "Test-Token",
                ApplicationName = "Console-Sample",
                Host = Environment.MachineName
            };

            ExceptionHandler.Configure(writter, defaults, null);
        }

        private static Exception CreateSampleException()
        {
            Exception exception = null;
            try
            {
                int i = 0;
                int result = 10 / i;
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Add some exception data.
            exception.Data.Add("Some-Key", "Some-Value");

            return exception;
        }

        private static IDictionary<string, object> CreateSampleProperties()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Test", "Value 1");
            properties.Add("Key", "Value 1");
            return properties;
        }
    }
}
