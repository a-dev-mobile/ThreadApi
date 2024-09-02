using Microsoft.AspNetCore.Mvc;
using Serilog;
using ThreadApi.Common.Services;
using ThreadApi.Features.MetricThread.Models;
using ThreadApi.Features.MetricThread.Services;

namespace ThreadApi.Features.MetricThread.Controllers
{
    [ApiController]
    [Route("api/v1/metric/diameters")]
    public class MetricThreadController : ControllerBase
    {
        private readonly MetricThreadService _metricThreadService;

        public MetricThreadController(MetricThreadService metricThreadService)
        {
            _metricThreadService = metricThreadService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DiameterModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDiameters([FromQuery] string order = "asc")
        {
            try
            {
                List<DiameterModel> data = await _metricThreadService.GetMetricDiameters(order);
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
