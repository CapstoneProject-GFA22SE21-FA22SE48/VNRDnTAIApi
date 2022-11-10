using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class DecreesController : ControllerBase
    {
        private readonly DecreeBusinessEntity _entity;

        public DecreesController(IUnitOfWork work)
        {
            _entity = new DecreeBusinessEntity(work);
        }

        // GET: api/Decrees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Decree>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Decree>>> GetDecrees()
        {
            try
            {
                return StatusCode(200, await _entity.GetDecreesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
