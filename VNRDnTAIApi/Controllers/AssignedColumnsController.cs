using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.ManageTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Authorize]
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
        // GET: api/AssignedColumns/Tasks
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
        //-----------------------------------------
        //POST: api/AssignedColumns/TasksUpdate
        [HttpPost("TasksUpdate")]
        [ProducesResponseType(typeof(IEnumerable<TaskDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> AdminUpdateTasks(IEnumerable<TaskDTO> taskDTOList)
        {
            try
            {
                return StatusCode(200, await _entity.AdminUpdateAssignTasks(taskDTOList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
