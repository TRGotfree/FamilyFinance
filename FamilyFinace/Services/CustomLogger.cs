using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using FamilyFinace.Interfaces;

namespace FamilyFinace.Services
{
    public class CustomLogger : ICustomLogger
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void LogError(Exception ex)
        {
            logger.Error(ex);
        }

        public void LogError(string errorMessage)
        {
            logger.Error(errorMessage);
        }

        public void LogInfo(string infoMessage)
        {
            logger.Info(infoMessage);
        }

        public void LogWarn(string warningMessage)
        {
            logger.Warn(warningMessage);
        }
    }
}
