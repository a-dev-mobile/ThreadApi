using Microsoft.AspNetCore.Mvc;
using Serilog;
using ThreadApi.Common.Services;
using ThreadApi.Features.MetricThread.Models;

namespace ThreadApi.Features.MetricThread.Controllers
{
    [ApiController]
    [Route("api/metric/diameters")]
    [ApiVersion("1.0")]
    public class MetricThreadController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public MetricThreadController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<DiameterModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDiameters([FromQuery] string order = "asc")
        {
            try
            {
                List<DiameterModel> data = await _databaseService.GetMetricDiameters(order);
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
