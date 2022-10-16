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
    public class AssignedSignCategoriesController : ControllerBase
    {
        private readonly AssignedSignCategoryBusinessEntity _entity;

        public AssignedSignCategoriesController(IUnitOfWork work)
        {
            _entity = new AssignedSignCategoryBusinessEntity(work);
        }

        // GET: api/AssignedSignCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssignedSignCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedSignCategory>>> GetAssignedSignCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedSignCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/AssignedSignCategories/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<AssignedSignCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedSignCategory>>> GetAssignedSignCategoriesByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedSignCategoriesByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
