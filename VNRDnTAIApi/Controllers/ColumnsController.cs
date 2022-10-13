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

    public class ColumnsController : ControllerBase
    {
        private readonly ColumnBusinessEntity _entity;

        public ColumnsController(IUnitOfWork work)
        {
            _entity = new ColumnBusinessEntity(work);
        }

        // GET: api/Columns
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Column>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Column>>> GetColumns()
        {
            try
            {
                return StatusCode(200, await _entity.GetColumnsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Columns/AssigendColumn/5
        [HttpGet("AssignedColumn/{columnId}")]
        [ProducesResponseType(typeof(IEnumerable<Column>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Column>>> GetAssignedColumn(Guid columnId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedColumnsAsync(columnId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Columns/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Column), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Column>> GetColumn(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetColumnAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Columns/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Column), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutColumn(Guid id, Column column)
        {
            if (id != column.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateColumn(column));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Columns
        [HttpPost]
        [ProducesResponseType(typeof(Column), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Column>> PostColumn(Column column)
        {
            try
            {
                return StatusCode(201, await _entity.AddColumn(column));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Columns/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteColumn(Guid id)
        {
            try
            {
                await _entity.RemoveColumn(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
