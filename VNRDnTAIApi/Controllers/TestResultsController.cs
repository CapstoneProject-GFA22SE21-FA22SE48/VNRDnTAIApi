using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/TestResults/GetTestResultByUserId
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

        // GET: api/TestResults/GetTestAttemptDTOs
        [Authorize]
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

        // POST: api/TestResults
        [Authorize]
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
    }
}
