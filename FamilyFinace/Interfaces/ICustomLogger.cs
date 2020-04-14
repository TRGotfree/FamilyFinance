using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface ICustomLogger
    {
        void LogError(Exception ex);

        void LogError(string errorMessage);

        void LogWarn(string warningMessage);

        void LogInfo(string infoMessage);
    }
}
