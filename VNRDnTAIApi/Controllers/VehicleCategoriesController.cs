using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class VehicleCategoriesController : ControllerBase
    {
        private readonly VehicleCategoryBusinessEntity _entity;

        public VehicleCategoriesController(IUnitOfWork work)
        {
            _entity = new VehicleCategoryBusinessEntity(work);
        }

        // GET: api/VehicleCategories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleCategory>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<VehicleCategory>>> GetVehicleCategories()
        {
            try
            {
                return StatusCode(200, await _entity.GetVehicleCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
