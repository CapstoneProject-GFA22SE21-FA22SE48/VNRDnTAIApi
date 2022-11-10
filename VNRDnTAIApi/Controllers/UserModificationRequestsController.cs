using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UserModificationRequestsController : ControllerBase
    {
        private readonly UserModificationRequestBusinessEntity _entity;

        public UserModificationRequestsController(IUnitOfWork work)
        {
            _entity = new UserModificationRequestBusinessEntity(work);
        }
        // GET: api/UserModificationRequests/UserROMDetail/5
        [HttpGet("UserROMDetail/{modifyingUserId}")]
        [ProducesResponseType(typeof(UserModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>> GetUserRomDetail(Guid modifyingUserId)
        {
            try
            {
                return StatusCode(200, await _entity.GetUserRomDetail(modifyingUserId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/UserModificationRequests/Approve/5
        [HttpPost("Approve/{modifyingUserId}")]
        [ProducesResponseType(typeof(UserModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserModificationRequest>> ApproveUserROM(Guid modifyingUserId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveUserRom(modifyingUserId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/UserModificationRequests/Deny/5
        [HttpPost("Deny/{modifyingUserId}")]
        [ProducesResponseType(typeof(UserModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserModificationRequest>> DenyUserROM(Guid modifyingUserId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenyUserRom(modifyingUserId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
