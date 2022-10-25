using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.ManageTasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AssignedColumnsController : ControllerBase
    {
        private readonly AssignedColumnBusinessEntity _entity;

        public AssignedColumnsController(IUnitOfWork work)
        {
            _entity = new AssignedColumnBusinessEntity(work);
        }

        // GET: api/AssignedColumns
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssignedColumn>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedColumn>>> GetAssignedColumns()
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedColumnsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/AssignedColumns/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<AssignedColumn>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AssignedColumn>>> GetAssignedColumnsByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAssignedColumnsByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //-----------------------------------------
        [HttpGet("Tasks")]
        [ProducesResponseType(typeof(IEnumerable<TaskDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTask()
        {
            try
            {
                return StatusCode(200, await _entity.GetTasksAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
