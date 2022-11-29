using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Authorize]
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

        // GET: api/Columns/AssignedColumn/5
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
    }
}
