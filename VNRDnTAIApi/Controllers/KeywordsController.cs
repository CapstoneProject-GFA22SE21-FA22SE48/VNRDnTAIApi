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

    public class KeywordsController : ControllerBase
    {
        private readonly KeywordBusinessEntity _entity;

        public KeywordsController(IUnitOfWork work)
        {
            _entity = new KeywordBusinessEntity(work);
        }

        // GET: api/Keywords
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Keyword>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Keyword>>> GetKeywords()
        {
            try
            {
                return StatusCode(200, await _entity.GetKeywordsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Keywords/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Keyword), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Keyword>> GetKeyword(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetKeywordAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Keywords/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Keyword), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutKeyword(Guid id, Keyword keyword)
        {
            if (id != keyword.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateKeyword(keyword));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Keywords
        [HttpPost]
        [ProducesResponseType(typeof(Keyword), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Keyword>> PostKeyword(Keyword keyword)
        {
            try
            {
                return StatusCode(201, await _entity.AddKeyword(keyword));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Keywords/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteKeyword(Guid id)
        {
            try
            {
                await _entity.RemoveKeyword(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
