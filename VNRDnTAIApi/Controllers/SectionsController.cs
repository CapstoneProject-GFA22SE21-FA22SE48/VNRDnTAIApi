using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.SearchLaw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class SectionsController : ControllerBase
    {
        private readonly SectionBusinessEntity _entity;

        public SectionsController(IUnitOfWork work)
        {
            _entity = new SectionBusinessEntity(work);
        }

        // GET: api/Sections/ByStatue/5
        [Authorize]
        [HttpGet("ByStatue/{statueId}")]
        [ProducesResponseType(typeof(IEnumerable<Section>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Section>>> GetSectionsByStatueId(Guid statueId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSectionsByStatueIdAsync(statueId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/Sections/GetSearchListByQuery
        [AllowAnonymous]
        [HttpGet("GetSearchListByQuery")]
        [ProducesResponseType(typeof(SearchLawDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SearchLawDTO>> GetSearchListByQuery([FromQuery] string query, [FromQuery] string vehicleCategory)
        {
            try
            {
                return StatusCode(200, await _entity.GetSearchListByQuery(query, vehicleCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/Sections/GetSearchListByKeywordId
        [AllowAnonymous]
        [HttpGet("GetSearchListByKeywordId")]
        [ProducesResponseType(typeof(SearchLawDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SearchLawDTO>> GetSearchListByKeywordId([FromQuery] Guid keywordId, [FromQuery] string vehicleCategory)
        {
            try
            {
                return StatusCode(200, await _entity.GetSearchListByKeywordId(keywordId, vehicleCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Sections
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Section), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Section>> PostSection(Section section)
        {
            try
            {
                return StatusCode(201, await _entity.AddSectionForROM(section));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Sections/NewSection
        [Authorize]
        [HttpPost("NewSection")]
        [ProducesResponseType(typeof(Section), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Section>> CreateNewSection(NewSectionDTO newSectionDTO)
        {
            try
            {
                return StatusCode(201, await _entity.CreateNewSection(newSectionDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
