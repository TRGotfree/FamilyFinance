using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IReportProvider
    {
        Task<byte[]> GetReportData<T>(IEnumerable<T> data, string reportName);
    }
}
