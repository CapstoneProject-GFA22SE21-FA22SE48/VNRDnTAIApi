using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class QuestionCategoriesController : ControllerBase
    {
        private readonly QuestionCategoryBusinessEntity _entity;

        public QuestionCategoriesController(IUnitOfWork work)
        {
            _entity = new QuestionCategoryBusinessEntity(work);
        }

        //GET: api/QuestionCategories/GetQuestionCategoriesByTestCategoryId/5
        [AllowAnonymous]
        [HttpGet("GetQuestionCategoriesByTestCategoryId/{testCategoryId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestionCategoryDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<QuestionCategoryDTO>>> GetQuestionCategoriesByTestCategoryId(Guid testCategoryId)
        {
            try
            {
                return StatusCode(200, await _entity.GetQuestionCategoriesByTestCategoryId(testCategoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
