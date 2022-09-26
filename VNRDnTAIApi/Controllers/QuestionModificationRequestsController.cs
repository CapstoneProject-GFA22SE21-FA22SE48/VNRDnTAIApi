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

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class QuestionModificationRequestsController : ControllerBase
    {
        private readonly QuestionModificationRequestBusinessEntity _entity;

        public QuestionModificationRequestsController(IUnitOfWork work)
        {
            _entity = new QuestionModificationRequestBusinessEntity(work);
        }

        // GET: api/QuestionModificationRequests
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuestionModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<QuestionModificationRequest>>> GetQuestionModificationRequests()
        {
            try
            {
                return StatusCode(200, await _entity.GetQuestionModificationRequestsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/QuestionModificationRequests/ModifyingQuestions/5
        [HttpGet("ModifyingQuestions/{modifyingQuestionId}")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>>
            GetQuestionModificationRequestByModifyingQuestionId(Guid modifyingQuestionId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetQuestionModificationRequestByModifyingQuestionIdAsync(modifyingQuestionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/QuestionModificationRequests/ModifiedQuestions/5
        [HttpGet("ModifiedQuestions/{modifiedQuestionId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestionModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<QuestionModificationRequest>>>
            GetQuestionModificationRequestsByModifiedQuestionId(Guid modifiedQuestionId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetQuestionModificationRequestsByModifiedQuestionIdAsync(modifiedQuestionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/QuestionModificationRequests/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestionModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<QuestionModificationRequest>>>
            GetQuestionModificationRequestsByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetQuestionModificationRequestsByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/QuestionModificationRequests/Questions/5/Users/5
        [HttpPut("Questions/{questionId}/Users/{userid}")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult>
            PutQuestionModificationRequest(Guid modifyingQuestionId, Guid scribeId, QuestionModificationRequest questionModificationRequest)
        {
            if (modifyingQuestionId != questionModificationRequest.ModifyingQuestionId || scribeId != questionModificationRequest.ScribeId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateQuestionModificationRequest(questionModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/QuestionModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(QuestionModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>> PostQuestionModificationRequest(QuestionModificationRequest questionModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.AddQuestionModificationRequest(questionModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/QuestionModificationRequests/Questions/5
        [HttpDelete("Questions/{modifyingQuestionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteQuestionModificationRequest(Guid modifyingQuestionId)
        {
            try
            {
                await _entity.RemoveQuestionModificationRequest(modifyingQuestionId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
