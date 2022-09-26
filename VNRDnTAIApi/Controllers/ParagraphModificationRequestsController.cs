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

    public class ParagraphModificationRequestsController : ControllerBase
    {
        private readonly ParagraphModificationRequestBusinessEntity _entity;

        public ParagraphModificationRequestsController(IUnitOfWork work)
        {
            _entity = new ParagraphModificationRequestBusinessEntity(work);
        }

        // GET: api/ParagraphModificationRequests
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ParagraphModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphModificationRequest>>> GetParagraphModificationRequests()
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphModificationRequestsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ParagraphModificationRequests/ModifyingParagraphs/5
        [HttpGet("ModifyingParagraphs/{modifyingParagraphId}")]
        [ProducesResponseType(typeof(ParagraphModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParagraphModificationRequest>>
            GetParagraphModificationRequestByModifyingParagraphId(Guid modifyingParagraphId)
        {
            try
            {
                return StatusCode(200, 
                    await _entity.GetParagraphModificationRequestByModifyingParagraphIdAsync(modifyingParagraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ParagraphModificationRequests/ModifiedParagraphs/5
        [HttpGet("ModifiedParagraphs/{modifiedParagraphId}")]
        [ProducesResponseType(typeof(IEnumerable<ParagraphModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphModificationRequest>>> 
            GetParagraphModificationRequestsByModifiedParagraphId(Guid modifiedParagraphId)
        {
            try
            {
                return StatusCode(200, 
                    await _entity.GetParagraphModificationRequestsByModifiedParagraphIdAsync(modifiedParagraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ParagraphModificationRequests/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<ParagraphModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphModificationRequest>>>
            GetParagraphModificationRequestsByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphModificationRequestsByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/ParagraphModificationRequests/Paragraphs/5/Users/5
        [HttpPut("Paragraphs/{paragraphId}/Users/{userid}")]
        [ProducesResponseType(typeof(ParagraphModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> 
            PutParagraphModificationRequest(Guid modifyingParagraphId, Guid scribeId, ParagraphModificationRequest paragraphModificationRequest)
        {
            if (modifyingParagraphId != paragraphModificationRequest.ModifyingParagraphId || scribeId != paragraphModificationRequest.ScribeId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateParagraphModificationRequest(paragraphModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/ParagraphModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(ParagraphModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParagraphModificationRequest>> PostParagraphModificationRequest(ParagraphModificationRequest paragraphModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.AddParagraphModificationRequest(paragraphModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/ParagraphModificationRequests/Paragraphs/5
        [HttpDelete("Paragraphs/{modifyingParagraphId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteParagraphModificationRequest(Guid modifyingParagraphId)
        {
            try
            {
                await _entity.RemoveParagraphModificationRequest(modifyingParagraphId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
