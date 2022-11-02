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
using DTOsLibrary;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class TestResultsController : ControllerBase
    {
        private readonly TestResultBusinessEntity _entity;

        public TestResultsController(IUnitOfWork work)
        {
            _entity = new TestResultBusinessEntity(work);
        }

        // GET: api/TestResults
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TestResult>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResults()
        {
            try
            {
                return StatusCode(200, await _entity.GetTestResultsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/TestResults/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TestResult), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestResult>> GetTestResult(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetTestResultAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetTestResultByUserId")]
        [ProducesResponseType(typeof(TestResult), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestResult>> GetTestResultByUserId([FromQuery] Guid userId, [FromQuery] Guid testCategoryId)
        {
            try
            {
                return StatusCode(200, await _entity.GetTestResultByUserId(userId, testCategoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetTestAttemptDTOs")]
        [ProducesResponseType(typeof(TestResult), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestAttempDTO>> GetTestAttemptDTOs([FromQuery] Guid testResultId, [FromQuery] Guid userId, [FromQuery] Guid testCategoryId)
        {
            try
            {
                return StatusCode(200, await _entity.GetTestAttemptDTOs(testResultId, userId, testCategoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/TestResults/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TestResult), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutTestResult(Guid id, TestResult testResult)
        {
            if (id != testResult.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateTestResult(testResult));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/TestResults
        [HttpPost]
        [ProducesResponseType(typeof(TestResult), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestResult>> PostTestResult(TestResult testResult)
        {
            try
            {
                return StatusCode(201, await _entity.AddTestResult(testResult));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TestResults/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTestResult(Guid id)
        {
            try
            {
                await _entity.RemoveTestResult(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TestResults/5
        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetIncorrectQuestionsOfTestResults(Guid id)
        {
            try
            {
                await _entity.RemoveTestResult(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
