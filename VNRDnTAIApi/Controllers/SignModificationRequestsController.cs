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

        // GET: api/SignModificationRequests/Scribes/5/2
        [HttpGet("Scribes/{scribeId}/{status}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>>
            GetSignModificationRequestsByScribeIdAndStatus(Guid scribeId, int status)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsByScribeIdAndStatusAsync(scribeId, status));
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
        // GET: api/SignModificationRequests/GpssignROMs
        [HttpGet("GpssignROMs/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>> GetGpssignRoms(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetGpssignRoms(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/SignModificationRequests/GPSSigns/5
        [HttpPut("GPSSigns/{gpsSignRomId}/{status}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult>
            ConfirmSignModificationRequest(Guid gpsSignRomId, int status, [FromBody] string imageUrl)
        {   // Expected: { status: 3 (Status.Confirmed), required imageUrl }
            SignModificationRequest signModificationRequest;
            try
            {
                signModificationRequest = await _entity.GetSignModificationRequestAsyncById(gpsSignRomId);
                if (signModificationRequest != null)
                {
                    signModificationRequest.ImageUrl = imageUrl;
                    signModificationRequest.Status = status;

                    return StatusCode(200, await _entity.UpdateSignModificationRequest(signModificationRequest));
                }
                else
                {
                    return StatusCode(404, "Request could not be found!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // PUT: api/SignModificationRequests/GPSSign/Claim/5
        [HttpPut("GPSSign/Claim/{modifyingGpssignId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SignModificationRequest>> ClaimGpssignRom(Guid modifyingGpssignId, SignModificationRequest gpsSignRom)
        {
            if (modifyingGpssignId != gpsSignRom.ModifyingGpssignId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.ClaimGpssignRom(gpsSignRom));
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
    }
}
