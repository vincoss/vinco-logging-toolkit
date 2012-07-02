using System;
using System.Collections;

using Elmah.Everywhere.Diagnostics;
using Elmah.Everywhere.Configuration;
using System.Configuration;
using System.IO;


namespace Elmah.Everywhere
{
    /// <summary>
    /// Logs an error to a remote site.
    /// </summary>
    public sealed class HttpErrorLog : ErrorLog
    {
        private readonly IDictionary _config;

        public HttpErrorLog(IDictionary config)
        {
            if(config == null)
            {
                throw new ArgumentNullException("config");
            }
            _config = config;
        }

        public override ErrorLogEntry GetError(string id)
        {
            throw new NotSupportedException();
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            throw new NotSupportedException();
        }

        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            Write(error.Exception.ToString());
            return ExceptionHandler.Report(error.Exception).Id;
        }

        /// <summary>
        /// Error log name. Logs an error to a remote site.
        /// </summary>
        public override string Name
        {
            get { return "Http Error Log"; }
        }

        // TODO: remove this
        private static void Write(string text)
        {
            var configuration =
               (EverywhereConfigurationSection)
               ConfigurationManager.GetSection(EverywhereConfigurationSection.SECTION_KEY);
            if (configuration == null)
            {
                throw new InvalidOperationException(string.Format("Could not find [{0}] configuration section.",
                                                                  EverywhereConfigurationSection.SECTION_KEY));
            }



            var writer = new StreamWriter(new FileStream(configuration.FileLogPath, FileMode.Append, FileAccess.Write, FileShare.Read), System.Text.Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
            writer.Close();

        }
    }
}