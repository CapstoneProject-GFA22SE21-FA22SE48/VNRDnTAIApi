using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: api/Answers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Answer>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            try
            {
                return StatusCode(200, await _entity.GetAnswersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        // GET: api/Answers/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Answer), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Answer>> GetAnswer(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetAnswerAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Answers/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Answer), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutAnswer(Guid id, Answer answer)
        {
            if (id != answer.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateAnswer(answer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Answers
        [HttpPost]
        [ProducesResponseType(typeof(Answer), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            try
            {
                return StatusCode(201, await _entity.AddAnswer(answer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Answers/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            try
            {
                await _entity.RemoveAnswer(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
