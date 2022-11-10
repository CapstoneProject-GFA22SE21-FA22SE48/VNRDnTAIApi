using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.SearchLaw;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ParagraphsController : ControllerBase
    {
        private readonly ParagraphBusinessEntity _entity;

        public ParagraphsController(IUnitOfWork work)
        {
            _entity = new ParagraphBusinessEntity(work);
        }

        // GET: api/Paragraphs/BySection/5
        [HttpGet("BySection/{sectionId}")]
        [ProducesResponseType(typeof(IEnumerable<ParagraphDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ParagraphDTO>>> GetParagraphsBySectionId(Guid sectionId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphsBySectionIdAsync(sectionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSearchParagraphDTOAsync")]
        [ProducesResponseType(typeof(SearchLawDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SearchLawDTO>> GetSearchParagraphDTOAsync([FromQuery] Guid paragraphId)
        {
            try
            {
                return StatusCode(200, await _entity.GetSearchParagraphDTOAsync(paragraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Paragraphs
        [HttpPost]
        [ProducesResponseType(typeof(ParagraphDTO), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParagraphDTO>> PostParagraph(ParagraphDTO paragraphDTO)
        {
            try
            {
                return StatusCode(201, await _entity.AddParagraph(paragraphDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
