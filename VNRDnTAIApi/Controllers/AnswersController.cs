using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AnswersController : ControllerBase
    {
        private readonly AnswerBusinessEntity _entity;

        public AnswersController(IUnitOfWork work)
        {
            _entity = new AnswerBusinessEntity(work);
        }

        // GET: api/Answers/Question
        [HttpGet("Question/{questionId}")]
        [ProducesResponseType(typeof(IEnumerable<Answer>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswersByQuestionId(Guid questionId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAnswersByQuestionIdAsync(questionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
