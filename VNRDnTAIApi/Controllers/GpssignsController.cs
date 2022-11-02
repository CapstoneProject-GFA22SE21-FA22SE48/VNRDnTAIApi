using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DTOsLibrary;

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

        // GET: api/Gpssigns
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Gpssign>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Gpssign>>> GetGpssigns()
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Gpssigns/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Gpssign), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Gpssign>> GetGpssign(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Gpssigns/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Gpssign), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutGpssign(Guid id, Gpssign gpssign)
        {
            if (id != gpssign.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateGpssign(gpssign));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Gpssigns
        [HttpPost]
        [ProducesResponseType(typeof(Gpssign), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Gpssign>> PostGpssign(Gpssign gpssign)
        {
            try
            {
                return StatusCode(201, await _entity.AddGpssign(gpssign));
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
        public async Task<ActionResult<Gpssign>> PostGpsSignDTO([FromBody]GpsSignDTO gpsSignDTO)
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

        // DELETE: api/Gpssigns/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteGpssign(Guid id)
        {
            try
            {
                await _entity.RemoveGpssign(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Gpssigns/GetNearbyGpsSign
        [HttpGet("GetNearbyGpsSign")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNearbyGpsSign(double latitude, double longtitude, double distance)
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignsNearby(latitude, longtitude, distance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
