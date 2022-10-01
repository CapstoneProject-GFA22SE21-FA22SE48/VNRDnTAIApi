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

    public class TestResultDetailsController : ControllerBase
    {
        private readonly TestResultDetailBusinessEntity _entity;

        public TestResultDetailsController(IUnitOfWork work)
        {
            _entity = new TestResultDetailBusinessEntity(work);
        }

        // GET: api/TestResultDetails
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TestResultDetail>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TestResultDetail>>> GetTestResultDetails()
        {
            try
            {
                return StatusCode(200, await _entity.GetTestResultDetailsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/TestResultDetails/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TestResultDetail), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestResultDetail>> GetTestResultDetail(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetTestResultDetailAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/TestResultDetails/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TestResultDetail), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutTestResultDetail(Guid id, TestResultDetail testResultDetail)
        {
            if (id != testResultDetail.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateTestResultDetail(testResultDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/TestResultDetails
        [HttpPost]
        [ProducesResponseType(typeof(TestResultDetail), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TestResultDetail>> PostTestResultDetail(TestResultDetail testResultDetail)
        {
            try
            {
                return StatusCode(201, await _entity.AddTestResultDetail(testResultDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TestResultDetails/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTestResultDetail(Guid id)
        {
            try
            {
                await _entity.RemoveTestResultDetail(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
