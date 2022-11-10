using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class GpssignsController : ControllerBase
    {
        private readonly GpssignBusinessEntity _entity;

        public GpssignsController(IUnitOfWork work)
        {
            _entity = new GpssignBusinessEntity(work);
        }

        // GET: api/Gpssigns/GetNearbyGpsSign
        [HttpGet("GetNearbyGpsSign")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNearbyGpsSign(double latitude, double longitude, double distance)
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignsNearby(latitude, longitude, distance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Gpssigns/AddGpsSignDTO
        [HttpPost("AddGpsSignDTO")]
        [ProducesResponseType(typeof(Gpssign), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Gpssign>> PostGpsSignDTO([FromBody] GpsSignDTO gpsSignDTO)
        {
            try
            {
                return StatusCode(201, await _entity.AddGpsSignDTO(gpsSignDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
