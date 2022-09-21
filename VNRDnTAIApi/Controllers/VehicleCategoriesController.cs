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

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class VehicleCategoriesController : ControllerBase
    {
        private readonly VehicleCategoryBusinessEntity _entity;

        public VehicleCategoriesController(IUnitOfWork work)
        {
            _entity = new VehicleCategoryBusinessEntity(work);
        }

        // GET: api/VehicleCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<VehicleCategory>>> GetVehicleCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetVehicleCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/VehicleCategories/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VehicleCategory), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VehicleCategory>> GetVehicleCategory(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetVehicleCategoryAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/VehicleCategories/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(VehicleCategory), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutVehicleCategorie(Guid id, VehicleCategory vehicleCategory)
        {
            if (id != vehicleCategory.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateVehicleCategory(vehicleCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/VehicleCategories
        [HttpPost]
        [ProducesResponseType(typeof(VehicleCategory), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VehicleCategory>> PostVehicleCategorie(VehicleCategory vehicleCategory)
        {
            try
            {
                return StatusCode(201, await _entity.AddVehicleCategory(vehicleCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/VehicleCategories/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteVehicleCategory(Guid id)
        {
            try
            {
                await _entity.RemoveVehicleCategory(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
