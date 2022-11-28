using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class KeywordsController : ControllerBase
    {
        private readonly KeywordBusinessEntity _entity;

        public KeywordsController(IUnitOfWork work)
        {
            _entity = new KeywordBusinessEntity(work);
        }

        // GET: api/Keywords
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Keyword>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Keyword>>> GetKeywords()
        {
            try
            {
                return StatusCode(200, await _entity.GetKeywordsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
