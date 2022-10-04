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
using DTOsLibrary;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminReportsController : ControllerBase
    {
        private readonly UserBusinessEntity _userEntity;

        public AdminReportsController(IUnitOfWork work)
        {
            _userEntity = new UserBusinessEntity(work);
        }

        // GET: api/AdminReports/UserByYear
        [HttpGet("UserByYear")]
        [ProducesResponseType(typeof(AdminUserByYearDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AdminUserByYearDTO>> GetAdminUserByYear()
        {
            try
            {
                return StatusCode(200, await _userEntity.GetAdminUserByYearReport());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
