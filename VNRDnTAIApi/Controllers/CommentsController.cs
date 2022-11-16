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

    public class CommentsController : ControllerBase
    {
        private readonly CommentBusinessEntity _entity;

        public CommentsController(IUnitOfWork work)
        {
            _entity = new CommentBusinessEntity(work);
        }

        // GET: api/Comments/Members
        [HttpGet("Members")]
        [ProducesResponseType(typeof(IEnumerable<Comment>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetMembersComments(Guid memberId)
        {
            try
            {
                return StatusCode(200, await _entity.GetCommentsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Comments/All
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<UserCommentDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<UserCommentDTO>>> GetMembersCommentsMobile()
        {
            try
            {
                return StatusCode(200, await _entity.GetUsersComments());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Comments/Members/5
        [HttpGet("Members/{memberId}")]
        [ProducesResponseType(typeof(IEnumerable<Comment>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentByMemberId(Guid memberId)
        {
            try
            {
                return StatusCode(200, await _entity.GetCommentByMemberId(memberId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Comments/AverageRating
        [HttpGet("AverageRating")]
        [ProducesResponseType(typeof(Comment), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Comment>> GetAverageRating()
        {
            try
            {
                return StatusCode(200, await _entity.GetAverageRating());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Comment), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutComment(Guid id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            Comment existed = await _entity.GetCommentAsync(id);
            if (comment.UserId != existed.UserId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateComment(comment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Comments/5
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(Comment), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Comment>> AddUserComment(Guid userId, Comment commentDTO)
        {
            try
            {
                return StatusCode(201, await _entity.AddUserComment(userId, commentDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _entity.RemoveComment(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
