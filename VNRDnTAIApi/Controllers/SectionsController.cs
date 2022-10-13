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

    public class SectionsController : ControllerBase
    {
        private readonly SectionBusinessEntity _entity;

        public SectionsController(IUnitOfWork work)
        {
            _entity = new SectionBusinessEntity(work);
        }

        // GET: api/Sections
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Section>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Section>>> GetSections()
        {
            try
            {
                return StatusCode(200, await _entity.GetSectionsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Sections/ByStatue/5
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

        // GET: api/Sections/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Section), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Section>> GetSection(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetSectionAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Sections/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Section), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutSection(Guid id, Section section)
        {
            if (id != section.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateSection(section));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Sections
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

        // DELETE: api/Sections/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            try
            {
                await _entity.RemoveSection(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
