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

    public class SignsController : ControllerBase
    {
        private readonly SignBusinessEntity _entity;

        public SignsController(IUnitOfWork work)
        {
            _entity = new SignBusinessEntity(work);
        }

        // GET: api/Signs
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sign>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Sign>>> GetSigns()
        {
            try
            {
                return StatusCode(200, await _entity.GetSignsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Signs/BySignCategory
        [HttpGet("BySignCategory/{signCategoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Sign>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Sign>>> GetSignsBySignCategoryId(Guid signCategoryId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignsBySignCategoryIdAsync(signCategoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Signs/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sign), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Sign>> GetSign(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Signs/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Sign), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutSign(Guid id, Sign sign)
        {
            if (id != sign.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateSign(sign));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Signs
        [HttpPost]
        [ProducesResponseType(typeof(Sign), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Sign>> PostSign(Sign sign)
        {
            try
            {
                return StatusCode(201, await _entity.AddSign(sign));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Signs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSign(Guid id)
        {
            try
            {
                await _entity.RemoveSign(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
