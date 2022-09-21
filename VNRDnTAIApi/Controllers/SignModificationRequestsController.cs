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

        // GET: api/SignModificationRequests/Signs/5/Users/5
        [HttpGet("Signs/{signId}/Users/{userId}")]
        [ProducesResponseType(typeof(SignModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignModificationRequest>>
            GetSignModificationRequestBySignIdUserId(Guid signId, Guid userId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestBySignIdUserIdAsync(signId, userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignModificationRequests/Signs/5
        [HttpGet("Signs/{signId}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>>
            GetSignModificationRequestsBySignId(Guid signId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsBySignIdAsync(signId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignModificationRequests/Users/5
        [HttpGet("Users/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<SignModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignModificationRequest>>>
            GetSignModificationRequestsByUserId(Guid userId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignModificationRequestsByUserIdAsync(userId));
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
            PutSignModificationRequest(Guid signId, Guid userId, SignModificationRequest signModificationRequest)
        {
            if (signId != signModificationRequest.SignId || userId != signModificationRequest.UserId)
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

        // DELETE: api/SignModificationRequests/Signs/5/Users/5
        [HttpDelete("Signs/{signId}/Users/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSignModificationRequest(Guid signId, Guid userId)
        {
            try
            {
                await _entity.RemoveSignModificationRequest(signId, userId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
