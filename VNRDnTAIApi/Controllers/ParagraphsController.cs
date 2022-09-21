using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        [ProducesResponseType(typeof(Paragraph), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Paragraph>> PostParagraph(Paragraph paragraph)
        {
            try
            {
                return StatusCode(201, await _entity.AddParagraph(paragraph));
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
