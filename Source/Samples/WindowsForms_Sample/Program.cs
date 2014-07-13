using System;
using System.Windows.Forms;
using Elmah.Everywhere;
using Elmah.Everywhere.Diagnostics;


namespace WindowsForms_Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CodeSetupExceptionHandler();

            // Handling UI thread exceptions.
            Application.ThreadException += Application_ThreadException;

            // Force the unhandled exceptions to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Handling non-UI thread exceptions.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler.Report((Exception)e.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ExceptionHandler.Report(e.Exception);
        }

        private static void CodeSetupExceptionHandler()
        {
            var defaults = new ExceptionDefaults
            {
                Token = "Test-Token",
                ApplicationName = "WindowsForms-Sample",
                Host = "WindowsForms-Sample",
                RemoteLogUri = new Uri("http://localhost:11079/error/log", UriKind.Absolute)
            };

            ExceptionHandler.Configure(new HttpExceptionWritter(), defaults, null);
        }
    }
}
