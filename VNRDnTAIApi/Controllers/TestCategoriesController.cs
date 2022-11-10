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

    public class TestCategoriesController : ControllerBase
    {
        private readonly TestCategoryBusinessEntity _entity;

        public TestCategoriesController(IUnitOfWork work)
        {
            _entity = new TestCategoryBusinessEntity(work);
        }

        // GET: api/TestCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TestCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TestCategory>>> GetTestCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetTestCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // GET: api/TestCategories/Count/5
        [HttpGet("Count")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TestCategoryCountDTO>>> CountQuestionsByTestCategoryId()
        {
            try
            {
                return StatusCode(200,
                await _entity.CountQuestionsByTestCategoryId());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
