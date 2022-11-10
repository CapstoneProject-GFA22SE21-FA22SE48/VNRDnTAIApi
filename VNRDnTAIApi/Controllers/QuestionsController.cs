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

    public class QuestionsController : ControllerBase
    {
        private readonly QuestionBusinessEntity _entity;

        public QuestionsController(IUnitOfWork work)
        {
            _entity = new QuestionBusinessEntity(work);
        }

        // GET: api/Questions/AssignedQuestions/Scribes/5
        [HttpGet("AssignedQuestions/Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetAssignedQuestions(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAssigedQuestionsAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetStudySetByCategoryAndSeparator")]
        [ProducesResponseType(typeof(IEnumerable<Question>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Question>>> GetStudySetByCategoryAndSeparator([FromQuery] string categoryId, [FromQuery] string questionCategoryId, [FromQuery] int separator)
        {
            try
            {
                return StatusCode(200, await _entity.GetStudySetByCategoryAndSeparator(categoryId, questionCategoryId, separator));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetRandomTestSetByCategoryId")]
        [ProducesResponseType(typeof(IEnumerable<Question>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Question>>> GetRandomTestSetByCategory([FromQuery] string categoryId)
        {
            try
            {
                return StatusCode(200, await _entity.GetRandomTestSetByCategory(categoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Questions
        [HttpPost]
        [ProducesResponseType(typeof(Question), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            try
            {
                return StatusCode(201, await _entity.AddQuestionForROM(question));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
