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

        // GET: api/ParagraphModificationRequests/Paragraphs/5/Users/5
        [HttpGet("Paragraphs/{paragraphId}/Users/{userId}")]
        [ProducesResponseType(typeof(ParagraphModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParagraphModificationRequest>>
            GetParagraphModificationRequestByParagraphIdUserId(Guid paragraphId, Guid userId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphModificationRequestByParagraphIdUserIdAsync(paragraphId, userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ParagraphModificationRequests/Paragraphs/5
        [HttpGet("Paragraphs/{paragraphId}")]
        [ProducesResponseType(typeof(IEnumerable<ParagraphModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphModificationRequest>>> 
            GetParagraphModificationRequestsByParagraphId(Guid paragraphId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphModificationRequestsByParagraphIdAsync(paragraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ParagraphModificationRequests/Users/5
        [HttpGet("Users/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ParagraphModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphModificationRequest>>>
            GetParagraphModificationRequestsByUserId(Guid userId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphModificationRequestsByUserIdAsync(userId));
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
            PutParagraphModificationRequest(Guid paragraphId, Guid userid, ParagraphModificationRequest paragraphModificationRequest)
        {
            if (paragraphId != paragraphModificationRequest.ParagraphId || userid != paragraphModificationRequest.UserId)
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

        // DELETE: api/ParagraphModificationRequests/Paragraphs/5/Users/5
        [HttpDelete("Paragraphs/{paragraphId}/Users/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteParagraphModificationRequest(Guid paragraphId, Guid userId)
        {
            try
            {
                await _entity.RemoveParagraphModificationRequest(paragraphId, userId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
