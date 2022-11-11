using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.SearchSign;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class SignCategoriesController : ControllerBase
    {
        private readonly SignCategoryBusinessEntity _entity;

        public SignCategoriesController(IUnitOfWork work)
        {
            _entity = new SignCategoryBusinessEntity(work);
        }

        //GET: api/SignCategories/GetSignCategoriesDTOList
        [HttpGet("GetSignCategoriesDTOList")]
        [ProducesResponseType(typeof(IEnumerable<SignCategoryDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignCategoryDTO>>> GetSignCategoriesDTOList()
        {
            try
            {
                return StatusCode(200, await _entity.GetSignCategoriesDTOList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/SignCategories/AssignedSignCategories/Scribes/5
        [HttpGet("AssignedSignCategories/Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<SignCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SignCategory>>> GetScribeAssignedSignCategoriesAsync(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetScribeAssignedSignCategoriesAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
