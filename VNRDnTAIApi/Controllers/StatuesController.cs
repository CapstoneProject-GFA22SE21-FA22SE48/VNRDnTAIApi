using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class StatuesController : ControllerBase
    {
        private readonly StatueBusinessEntity _entity;

        public StatuesController(IUnitOfWork work)
        {
            _entity = new StatueBusinessEntity(work);
        }

        // GET: api/Statues
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Statue>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Statue>>> GetStatues()
        {
            try
            {
                return StatusCode(200, await _entity.GetStatuesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Statues/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Statue), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Statue>> GetStatue(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetStatueAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Statues/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Statue), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutStatue(Guid id, Statue statue)
        {
            if (id != statue.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateStatue(statue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Statues
        [HttpPost]
        [ProducesResponseType(typeof(Statue), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Statue>> PostStatue(Statue statue)
        {
            try
            {
                return StatusCode(201, await _entity.AddStatueForROM(statue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Statues/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteStatue(Guid id)
        {
            try
            {
                await _entity.RemoveStatue(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
