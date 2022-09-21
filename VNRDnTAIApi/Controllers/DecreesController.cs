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

    public class DecreesController : ControllerBase
    {
        private readonly DecreeBusinessEntity _entity;

        public DecreesController(IUnitOfWork work)
        {
            _entity = new DecreeBusinessEntity(work);
        }

        // GET: api/Decrees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Decree>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Decree>>> GetDecrees()
        {
            try
            {
                return StatusCode(200, await _entity.GetDecreesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Decrees/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Decree), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Decree>> GetDecree(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetDecreeAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Decrees/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Decree), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutDecree(Guid id, Decree decree)
        {
            if (id != decree.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateDecree(decree));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Decrees
        [HttpPost]
        [ProducesResponseType(typeof(Decree), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Decree>> PostDecree(Decree decree)
        {
            try
            {
                return StatusCode(201, await _entity.AddDecree(decree));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Decrees/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDecree(Guid id)
        {
            try
            {
                await _entity.RemoveDecree(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
