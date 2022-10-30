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
using DTOsLibrary;

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

        // GET: api/TestCategories/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TestCategory), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestCategory>> GetTestCategory(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetTestCategoryAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/TestCategories/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TestCategory), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutTestCategory(Guid id, TestCategory testCategory)
        {
            if (id != testCategory.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateTestCategory(testCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/TestCategories
        [HttpPost]
        [ProducesResponseType(typeof(TestCategory), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestCategory>> PostTestCategory(TestCategory testCategory)
        {
            try
            {
                return StatusCode(201, await _entity.AddTestCategory(testCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TestCategories/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTestCategory(Guid id)
        {
            try
            {
                await _entity.RemoveTestCategory(id);
                return StatusCode(204);
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
