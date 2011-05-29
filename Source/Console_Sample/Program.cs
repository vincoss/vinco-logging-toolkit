using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vinco.ElmahHandler.Handlers;
using Vinco.ElmahHandler.Diagnostics;
using Vinco.ElmahHandler;



namespace Console_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure
            HttpHandler handler = new HttpHandler
            {
                RequestUri = new Uri("http://localhost:50003/error/put", UriKind.Absolute)
            };
            ExceptionHandler.SetWritter(new HttpExceptionWritter(handler));
            ExceptionHandler.SetParameters(new ExceptionParameters
            {
                Token = null,
                ApplicationName = "Exceptions-Handler",
                Host = "Console-Sample"
            });

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
