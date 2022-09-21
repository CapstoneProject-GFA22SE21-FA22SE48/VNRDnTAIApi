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

    [AllowAnonymous]
    public class SignParagraphsController : ControllerBase
    {
        private readonly SignParagraphBusinessEntity _entity;

        public SignParagraphsController(IUnitOfWork work)
        {
            _entity = new SignParagraphBusinessEntity(work);
        }

        // GET: api/SignParagraphs
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SignParagraph>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignParagraph>>> GetSignParagraphs()
        {
            try
            {
                return StatusCode(200, await _entity.GetSignParagraphsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignParagraphs/Signs/5
        [HttpGet("Signs/{signId}")]
        [ProducesResponseType(typeof(IEnumerable<SignParagraph>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignParagraph>>> GetSignParagraphsBySignId(Guid signId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignParagraphsBySignIdAsync(signId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignParagraphs/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SignParagraph), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignParagraph>> GetSignParagraph(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignParagraphAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/SignParagraphs/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SignParagraph), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutSignParagraph(Guid id, SignParagraph signParagraph)
        {
            if (id != signParagraph.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateSignParagraph(signParagraph));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/SignParagraphs
        [HttpPost]
        [ProducesResponseType(typeof(SignParagraph), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignParagraph>> PostSignParagraph(SignParagraph signParagraph)
        {
            try
            {
                return StatusCode(201, await _entity.AddSignParagraph(signParagraph));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/SignParagraphs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSignParagraph(Guid id)
        {
            try
            {
                await _entity.RemoveSignParagraph(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
