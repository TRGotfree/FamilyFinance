using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;


namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private const string excelFileContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly IReportProvider reportProvider;
        public ReportsController(ICustomLogger logger, IRepository repository, IReportProvider reportProvider)
        {
            this.logger = logger;
            this.repository = repository;
            this.reportProvider = reportProvider;
        }

        public async Task<IActionResult> GetCostsByPeriod(DateTime beginDate, DateTime endDate)
        {
            try
            {
                var costs = await repository.GetCosts(beginDate, endDate);
                if (costs == null || costs.Count == 0)
                    return StatusCode((int)HttpStatusCode.NoContent);

                var reportData = await reportProvider.GetReportData(costs, $"Отчет по расходам за период: {beginDate.ToString("dd.MM.yyyy")}-{endDate.ToString("dd.MM.yyyy")}");

                return File(reportData, excelFileContentType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, Constants.ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
