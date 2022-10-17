using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: api/Paragraphs
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Paragraph>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Paragraph>>> GetParagraphs()
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        // GET: api/Paragraphs/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Paragraph), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Paragraph>> GetParagraph(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Paragraphs/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Paragraph), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutParagraph(Guid id, Paragraph paragraph)
        {
            if (id != paragraph.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateParagraph(paragraph));
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

        // DELETE: api/Paragraphs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteParagraph(Guid id)
        {
            try
            {
                await _entity.RemoveParagraph(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
