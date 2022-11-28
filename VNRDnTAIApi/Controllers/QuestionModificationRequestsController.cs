using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Authorize]
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

        //---------------------------------------------------
        // GET: api/QuestionModificationRequests/QuestionROMDetail/5
        [HttpGet("QuestionROMDetail/{modifyingQuestionId}")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> GetQuestionRomDetail(Guid modifyingQuestionId)
        {
            try
            {
                return StatusCode(200, await _entity.GetQuestionRomDetail(modifyingQuestionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/QuestionModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(QuestionModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>>
            PostQuestionModificationRequest(QuestionModificationRequest questionModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.CreateQuestionModificationRequest(questionModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/QuestionModificationRequests/Approve/5
        [HttpPost("Approve/{modifyingQuestionId}")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>> ApproveQuestionROM(Guid modifyingQuestionId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveQuestionRom(modifyingQuestionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/QuestionModificationRequests/Deny/5
        [HttpPost("Deny/{modifyingQuestionId}")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>> DenyQuestionROM(Guid modifyingQuestionId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenyQuestionRom(modifyingQuestionId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/QuestionModificationRequests/Cancel/5
        [HttpPost("Cancel")]
        [ProducesResponseType(typeof(QuestionModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionModificationRequest>> CancelQuestionRom([FromBody] Guid modifyingQuestionId)
        {
            try
            {
                return StatusCode(200, await _entity.CancelQuestionRom(modifyingQuestionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // DELETE: api/QuestionModificationRequests/5
        [HttpDelete("{modifyingQuestionId}")]
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
