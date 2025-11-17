using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsDatabaseController : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsDatabaseController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;

        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportsRepository.GetReports();
            return Ok(new { Reports = reports });
        }

        [HttpGet("reports/{id}")]
        public async Task<IActionResult> GetReport(string id)
        {
            var report = await _reportsRepository.GetReport(id);
            return Ok(new { Report = report });
        }
    }
}
