using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elmah.Everywhere.Handlers;
using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere;




namespace Console_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure
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

            ExceptionHandler.WithParameters(defaults, writter);

            // Create exception and sample data.
            Exception exception = GetSampleException();
            exception.Data.Add("Some-Key", "Some-Value");

            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Test", "Value 1");

            // Report exception
            ExceptionHandler.Report(exception, properties);


            Console.WriteLine("Done...");
            Console.Read();
        }

        public static Exception GetSampleException()
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
            return exception;
        }
    }
}
