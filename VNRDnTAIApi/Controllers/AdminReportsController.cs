using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.AdminReport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminReportsController : ControllerBase
    {
        private readonly UserBusinessEntity _userEntity;

        public AdminReportsController(IUnitOfWork work)
        {
            _userEntity = new UserBusinessEntity(work);
        }

        // GET: api/AdminReports/MemberByYear
        [HttpGet("MemberByYear")]
        [ProducesResponseType(typeof(MemberByYearReportDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MemberByYearReportDTO>> GetMemberByYear()
        {
            try
            {
                return StatusCode(200, await _userEntity.GetMemberByYearReport());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/AdminReports/NewMember
        [HttpGet("NewMember/{month}/{year}")]
        [ProducesResponseType(typeof(NewMemberReportDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<NewMemberReportDTO>> GetNewMember(int month, int year)
        {
            try
            {
                return StatusCode(200, await _userEntity.GetNewMemberReport(month, year));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
