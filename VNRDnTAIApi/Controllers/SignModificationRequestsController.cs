using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class SignModificationRequestsController : ControllerBase
    {
        private readonly SignModificationRequestBusinessEntity _entity;

        public SignModificationRequestsController(IUnitOfWork work)
        {
            _entity = new SignModificationRequestBusinessEntity(work);
        }

        // GET: api/SignModificationRequests
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>> GetSignModificationRequests()
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignModificationRequests/Signs/5
        [HttpGet("Signs/{modifyingSignId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>>
            GetSignModificationRequestByModifyingSignId(Guid modifyingSignId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestByModifyingSignIdAsync(modifyingSignId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignModificationRequests/Signs/5
        [HttpGet("Signs/{modifiedSignId}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>>
            GetSignModificationRequestsByModifiedSignId(Guid modifiedSignId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsByModifiedSignIdAsync(modifiedSignId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignModificationRequests/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>>
            GetSignModificationRequestsByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/SignModificationRequests/Signs/5/Users/5
        [HttpPut("Signs/{signId}/Users/{userId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult>
            PutSignModificationRequest(Guid modifyingSignId, Guid scribeId, SignModificationRequest signModificationRequest)
        {
            if (modifyingSignId != signModificationRequest.ModifyingSignId || scribeId != signModificationRequest.ScribeId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateSignModificationRequest(signModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/SignModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(SignModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> PostSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.AddSignModificationRequest(signModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/SignModificationRequests/5
        [HttpDelete("{signRomId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSignModificationRequest(Guid signRomId)
        {
            try
            {
                await _entity.RemoveSignModificationRequest(signRomId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //---------------------------------------------------
        // GET: api/SignModificationRequests/SignROMDetail/5
        [HttpGet("SignROMDetail/{modifyingSignId}")]
        [ProducesResponseType(typeof(SignModificationRequestDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequestDTO>> GetSignRomDetail(Guid modifyingSignId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignRomDetail(modifyingSignId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //---------------------------------------------------
        // GET: api/SignModificationRequests/GpssignROMDetail/5
        [HttpGet("GpssignROMDetail/{modifyingGpssignId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> GetGpssignRomDetail(Guid modifyingGpssignId)
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignRomDetail(modifyingGpssignId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/SignModificationRequests/Approve/5
        [HttpPost("Approve/{modifyingSignId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> ApproveSignROM(Guid modifyingSignId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveSignRom(modifyingSignId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/SignModificationRequests/Deny/5
        [HttpPost("Deny/{modifyingSignId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> DenySignROM(Guid modifyingSignId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenySignRom(modifyingSignId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/SignModificationRequests/Cancel/5
        [HttpPost("Cancel")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> CancelSignRom([FromBody] Guid signRomId)
        {
            try
            {
                return StatusCode(200, await _entity.CancelSignRom(signRomId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
