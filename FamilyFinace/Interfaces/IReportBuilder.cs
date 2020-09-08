using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IReportBuilder
    {
        byte[] BuildReport<T>(string name, List<T> data);
    }
}
