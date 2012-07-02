using System;
using System.Web;
using Elmah.Everywhere.Web;
using Elmah.Everywhere.Diagnostics;

[assembly: PreApplicationStartMethod(typeof(Starter), "Start")]

namespace Elmah.Everywhere.Web
{
    public static class Starter
    {
        public static void Start()
        {
            // Configure error handler from configuration file
            ExceptionHandler.WithParameters(new HttpExceptionWritter());
        }
    }
}