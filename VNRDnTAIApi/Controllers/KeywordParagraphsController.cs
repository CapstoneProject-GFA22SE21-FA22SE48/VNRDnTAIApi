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

    public class KeywordParagraphsController : ControllerBase
    {
        private readonly KeywordParagraphBusinessEntity _entity;

        public KeywordParagraphsController(IUnitOfWork work)
        {
            _entity = new KeywordParagraphBusinessEntity(work);
        }

        // GET: api/KeywordParagraphs/Keywords/5
        [HttpGet("Keywords/{keywordId}")]
        [ProducesResponseType(typeof(IEnumerable<KeywordParagraph>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Keyword>>> GetKeywordParagraphByKeywordIds(Guid keywordId)
        {
            try
            {
                return StatusCode(200, await _entity.GetKeywordParagraphsByKeywordIdAsync(keywordId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
