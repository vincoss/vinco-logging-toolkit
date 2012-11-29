using System;


namespace Elmah.Everywhere
{
    public class ExceptionDefaults
    {
        /// <summary>
        /// Security token to authenticate logger.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The name of host machine where this error occurred.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The name of application in which this error occurred.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Remote web site url to log an error.
        /// </summary>
        public Uri RemoteLogUri { get; set; }
    }
}