using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class StatuesController : ControllerBase
    {
        private readonly StatueBusinessEntity _entity;

        public StatuesController(IUnitOfWork work)
        {
            _entity = new StatueBusinessEntity(work);
        }

        // GET: api/Statues
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Statue>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Statue>>> GetStatues()
        {
            try
            {
                return StatusCode(200, await _entity.GetStatuesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Statues
        [HttpPost]
        [ProducesResponseType(typeof(Statue), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Statue>> PostStatue(Statue statue)
        {
            try
            {
                return StatusCode(201, await _entity.AddStatueForROM(statue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
