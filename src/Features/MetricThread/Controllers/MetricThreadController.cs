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
        /// <summary>
        /// Gets the list of diameters.
        /// </summary>
        /// <returns>A list of <see cref="DiameterModel"/>.</returns>
        /// <response code="200">Returns the list of diameters.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<DiameterModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDiameters()
        {
            try
            {
                List<DiameterModel> data = await _databaseService.GetDiametersAsync("DESC");
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
