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

    public class UserModificationRequestsController : ControllerBase
    {
        private readonly UserModificationRequestBusinessEntity _entity;

        public UserModificationRequestsController(IUnitOfWork work)
        {
            _entity = new UserModificationRequestBusinessEntity(work);
        }

        // GET: api/UserModificationRequests
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<UserModificationRequest>>> GetUserModificationRequests()
        {
            try
            {
                return StatusCode(200, await _entity.GetUserModificationRequestsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/UserModificationRequests/ModifyingUsers/5
        [HttpGet("ModifyingUsers/{modifyingUserId}")]
        [ProducesResponseType(typeof(UserModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserModificationRequest>>
            GetUserModificationRequestByModifyingUserId(Guid modifyingUserId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetUserModificationRequestByModifyingUserIdAsync(modifyingUserId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/UserModificationRequests/ModifiedUsers/5
        [HttpGet("ModifiedUsers/{modifiedUserId}")]
        [ProducesResponseType(typeof(IEnumerable<UserModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<UserModificationRequest>>>
            GetUserModificationRequestsByModifiedUserId(Guid modifiedUserId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetUserModificationRequestsByModifiedUserIdAsync(modifiedUserId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/UserModificationRequests/Users/5
        [HttpPut("Users/{modifyingUserId}")]
        [ProducesResponseType(typeof(UserModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult>
            PutUserModificationRequest(Guid modifyingUserId, UserModificationRequest userModificationRequest)
        {
            if (modifyingUserId != userModificationRequest.ModifyingUserId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateUserModificationRequest(userModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/UserModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(UserModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserModificationRequest>> PostUserModificationRequest(UserModificationRequest userModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.AddUserModificationRequest(userModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/UserModificationRequests/Users/5
        [HttpDelete("Users/{modifyingUserId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserModificationRequest(Guid modifyingUserId)
        {
            try
            {
                await _entity.RemoveUserModificationRequest(modifyingUserId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
