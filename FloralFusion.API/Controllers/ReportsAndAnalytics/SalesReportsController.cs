using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using FloralFusion.Domain.Interfaces.ReportsAnsAnalytisctsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloralFusion.API.Controllers.ReportsAndAnalytics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="MANAGER,ADMIN")]
    public class SalesReportsController : ControllerBase
    {
        private readonly ISalesReportsService _salesReportsService;

        public SalesReportsController(ISalesReportsService _salesReportsService)
        {
            this._salesReportsService = _salesReportsService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<SalesReportModel>> GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            var res = await _salesReportsService.GenerateSalesReportAsync(startDate, endDate);

            if(res is null) return Response<SalesReportModel>.Error(ErrorKeys.BadRequest);

            return Response<SalesReportModel>.Ok(res);
        }

        [HttpGet]
        [Route(nameof(GetSalesReports))]
        public async Task<Response<IEnumerable<SalesReportModel>>> GetSalesReports()
        {
            var res=await _salesReportsService.GetSalesReportsAsync();
            if (!res.Any()) return Response<IEnumerable<SalesReportModel>>.Error(ErrorKeys.NotFound);
            return Response<IEnumerable<SalesReportModel>>.Ok(res);
        }

    }
}
