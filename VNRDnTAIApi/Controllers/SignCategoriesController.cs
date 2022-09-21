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

    public class SignCategoriesController : ControllerBase
    {
        private readonly SignCategoryBusinessEntity _entity;

        public SignCategoriesController(IUnitOfWork work)
        {
            _entity = new SignCategoryBusinessEntity(work);
        }

        // GET: api/SignCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SignCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignCategory>>> GetSignCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetSignCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignCategories/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SignCategory), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignCategory>> GetSignCategory(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignCategoryAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/SignCategories/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SignCategory), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutSignCategory(Guid id, SignCategory signCategory)
        {
            if (id != signCategory.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateSignCategory(signCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/SignCategories
        [HttpPost]
        [ProducesResponseType(typeof(SignCategory), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignCategory>> PostSignCategory(SignCategory signCategory)
        {
            try
            {
                return StatusCode(201, await _entity.AddSignCategory(signCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/SignCategories/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSignCategory(Guid id)
        {
            try
            {
                await _entity.RemoveSignCategory(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
