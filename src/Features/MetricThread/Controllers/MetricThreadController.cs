using Microsoft.AspNetCore.Mvc;
using Serilog;
using ThreadApi.Common.Services;
using ThreadApi.Features.MetricThread.Models;

namespace ThreadApi.Features.MetricThread.Controllers
{
    [ApiController]
    [Route("api/metric/diams")]
    [ApiVersion("1.0")]
    public class MetricThreadController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public MetricThreadController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiameters()
        {
            try
            {
                List<DiameterModel> data = await _databaseService.GetDiametersAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving diameters");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
