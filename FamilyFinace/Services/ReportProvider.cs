using FamilyFinace.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class ReportProvider : IReportProvider
    {
        public Task<byte[]> GetReportData<T>(IEnumerable<T> data, string reportName)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                var workSheet = excelPackage.Workbook.Worksheets.Add(reportName);
                workSheet.Cells["A1"].LoadFromCollection(data);

                return excelPackage.GetAsByteArrayAsync(); //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }
    }
}
