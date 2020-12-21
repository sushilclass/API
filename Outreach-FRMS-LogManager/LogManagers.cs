using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Outreach_FRMS_LogManager
{
    public class LogManagers
    {
        private static ILogger<LogManagers> logger;

        private static IOptions<ConfigLog> appSettings;

        public LogManagers(ILogger<LogManagers> loggers, IOptions<ConfigLog> app)
        {
            logger = loggers;
            appSettings = app;
        }
        /// <summary>
        /// Trace and write the Log in text file
        /// </summary>
        /// <param name="message"></param>
        public static void WriteTraceLog(StringBuilder message)
        {
            //// check the configur ation setting for write the trace log.
            //if (string.Compare(Convert.ToString(ConfigurationManager.AppSettings["IsTrace"], CultureInfo.CurrentCulture).ToUpper(CultureInfo.CurrentCulture), "True",
            //    true, CultureInfo.CurrentCulture) == 0 && message != null)
            //{
            if (appSettings.Value.IsTrace == true)
            {
                logger.LogInformation(Convert.ToString(message));
            }
            // }
        }
        /// <summary>
        /// Write the error log in text file
        /// </summary>
        /// <param name="objException"></param>
        public static void WriteErrorLog(Exception objException)
        {
            ////// check the configuration setting for write the error log.
            //if (string.Compare(ConfigurationManager.AppSettings["MakeErrorLog"].ToUpper(CultureInfo.CurrentCulture), "True", true, CultureInfo.CurrentCulture) == 0
            //    && objException != null)
            //{

            //bool yes = false;
            //if (yes == false)
            //{
            //    logger.LogError(objException, objException.Message.ToString());
            //}

            if(appSettings.Value.MakeErrorLog == true)
            {
                logger.LogError(objException, objException.Message.ToString());
            }
            // }
        }
    }
}
