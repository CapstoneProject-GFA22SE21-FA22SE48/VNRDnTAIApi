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

    public class SignsController : ControllerBase
    {
        private readonly SignBusinessEntity _entity;

        public SignsController(IUnitOfWork work)
        {
            _entity = new SignBusinessEntity(work);
        }

        // GET: api/Signs/GetSignByName
        [HttpGet("GetSignByName")]
        [ProducesResponseType(typeof(Sign), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Sign>> GetSignByName(string signName)
        {
            try
            {
                return StatusCode(200, await _entity.GetSignByName(signName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Signs/AssignedSigns/Scribes/5
        [HttpGet("AssignedSigns/Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<Sign>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Sign>>> GetScribeAssignedSigns(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetScribeAssignedSignsAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Signs
        [HttpPost]
        [ProducesResponseType(typeof(SignDTO), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SignDTO>> PostSign(SignDTO signDTO)
        {
            try
            {
                return StatusCode(201, await _entity.AddSignForROM(signDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
