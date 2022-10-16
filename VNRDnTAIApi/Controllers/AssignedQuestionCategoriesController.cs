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
    public class AssignedQuestionCategoriesController : ControllerBase
    {
        private readonly AssignedQuestionCategoryBusinessEntity _entity;

        public AssignedQuestionCategoriesController(IUnitOfWork work)
        {
            _entity = new AssignedQuestionCategoryBusinessEntity(work);
        }

        // GET: api/AssignedQuestionCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssignedQuestionCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedQuestionCategory>>> GetAssignedQuestionCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedQuestionCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/AssignedQuestionCategories/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<AssignedQuestionCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedQuestionCategory>>> GetAssignedQuestionCategoriesByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedQuestionCategoriesByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
