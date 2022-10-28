using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary.AdminReport;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminReportsController : ControllerBase
    {
        private readonly UserBusinessEntity _userEntity;
        private readonly LawModificationRequestBusinessEntity _lawRomEntity;

        public AdminReportsController(IUnitOfWork work)
        {
            _userEntity = new UserBusinessEntity(work);
            _lawRomEntity = new LawModificationRequestBusinessEntity(work);
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
        //---------------------------------------------------
        //GET: api/AdminReports/ROMReport/month/year/5
        [HttpGet("ROMReport/{month}/{year}/{adminId}")]
        [ProducesResponseType(typeof(RomReportDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RomReportDTO>> GetAdminROMReport(int month, int year, Guid adminId)
        {
            try
            {
                return StatusCode(200, await _lawRomEntity.GetAdminRomReportDTO(month, year, adminId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        //GET: api/AdminReports/ScribeReport/month/year
        [HttpGet("ScribeReport/{month}/{year}")]
        [ProducesResponseType(typeof(ScribeReportDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ScribeReportDTO>> GetAdminScribeReport(int month, int year)
        {
            try
            {
                return StatusCode(200, await _userEntity.GetAdminScribeReportDTO(month, year));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
