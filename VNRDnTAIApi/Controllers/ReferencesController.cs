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

    public class ReferencesController : ControllerBase
    {
        private readonly ReferenceBusinessEntity _entity;

        public ReferencesController(IUnitOfWork work)
        {
            _entity = new ReferenceBusinessEntity(work);
        }

        // GET: api/References
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reference>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Reference>>> GetReferences()
        {
            try
            {
                return StatusCode(200, await _entity.GetReferencesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/References/Paragraph/5
        [HttpGet("Paragraph/{paragraphId}")]
        [ProducesResponseType(typeof(Reference), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Reference>> GetReferencesByParagraphId(Guid paragraphId)
        {
            try
            {
                return StatusCode(200, await _entity.GetReferencesByParagraphIdAsync(paragraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/References/Paragraph/5/ReferenceParagraph/5
        [HttpPut("Paragraph/{paragraphId}/ReferenceParagraph/{referenceParagraphId}")]
        [ProducesResponseType(typeof(Reference), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutReference(Guid paragraphId, Guid referenceParagraphId, Reference reference)
        {
            if (paragraphId != reference.ParagraphId || referenceParagraphId != reference.ReferenceParagraphId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateReference(reference));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/References
        [HttpPost]
        [ProducesResponseType(typeof(Reference), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Reference>> PostReference(Reference reference)
        {
            try
            {
                return StatusCode(201, await _entity.AddReference(reference));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/References/Paragraph/5/ReferenceParagraph/5
        [HttpDelete("Paragraph/{paragraphId}/ReferenceParagraph/{referenceParagraphId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReference(Guid paragraphId, Guid referenceParagraphId)
        {
            try
            {
                await _entity.RemoveReference(paragraphId, referenceParagraphId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
