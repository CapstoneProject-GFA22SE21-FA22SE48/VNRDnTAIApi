﻿using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.ManageROM;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class LawModificationRequestsController : ControllerBase
    {
        private readonly LawModificationRequestBusinessEntity _entity;

        public LawModificationRequestsController(IUnitOfWork work)
        {
            _entity = new LawModificationRequestBusinessEntity(work);
        }

        // GET: api/LawModificationRequests
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LawModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LawModificationRequest>>> GetLawModificationRequests()
        {
            try
            {
                return StatusCode(200, await _entity.GetLawModificationRequestsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/LawModificationRequests/AdminROMList/5
        [HttpGet("AdminROMList/{adminId}")]
        [ProducesResponseType(typeof(AdminRomListDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AdminRomListDTO>> GetAdminRomList(Guid adminId)
        {
            try
            {
                return StatusCode(200, await _entity.GetAdminRomList(adminId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/LawModificationRequests/ScribeROMList/5
        [HttpGet("ScribeROMList/{scribeId}")]
        [ProducesResponseType(typeof(ScribeRomListDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ScribeRomListDTO>> GetScribeRomList(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetScribeRomList(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/LawModificationRequests/ModifyingParagraphs/5
        [HttpGet("ModifyingParagraphs/{modifyingParagraphId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>>
            GetLawModificationRequestByModifyingParagraphId(Guid modifyingParagraphId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetLawModificationRequestByModifyingParagraphIdAsync(modifyingParagraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/LawModificationRequests/ModifiedParagraphs/5
        [HttpGet("ModifiedParagraphs/{modifiedParagraphId}")]
        [ProducesResponseType(typeof(IEnumerable<LawModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LawModificationRequest>>>
            GetLawModificationRequestsByModifiedParagraphId(Guid modifiedParagraphId)
        {
            try
            {
                return StatusCode(200,
                    await _entity.GetLawModificationRequestsByModifiedParagraphIdAsync(modifiedParagraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/LawModificationRequests/Scribes/5
        [HttpGet("Scribes/{scribeId}")]
        [ProducesResponseType(typeof(IEnumerable<LawModificationRequest>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LawModificationRequest>>>
            GetLawModificationRequestsByScribeId(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetLawModificationRequestsByScribeIdAsync(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/LawModificationRequests/Paragraphs/5/Users/5
        [HttpPut("Paragraphs/{sectionId}/Users/{userid}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult>
            PutLawModificationRequest(Guid modifyingParagraphId, Guid scribeId, LawModificationRequest lawModificationRequest)
        {
            if (modifyingParagraphId != lawModificationRequest.ModifyingParagraphId || scribeId != lawModificationRequest.ScribeId)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateLawModificationRequest(lawModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/LawModificationRequests
        [HttpPost]
        [ProducesResponseType(typeof(LawModificationRequest), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> PostLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            try
            {
                return StatusCode(201, await _entity.AddLawModificationRequest(lawModificationRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/LawModificationRequests/5
        [HttpDelete("{lawRomId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLawModificationRequest(Guid lawRomId)
        {
            try
            {
                await _entity.RemoveLawModificationRequest(lawRomId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //---------------------------------------------------
        // GET: api/LawModificationRequests/ROMDetail/5
        [HttpGet("ROMDetail/{lawRomId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> GetLawRomDetail(Guid lawRomId)
        {
            try
            {
                return StatusCode(200, await _entity.GetLawRomDetail(lawRomId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/LawModificationRequests/ParagraphROMDetail/References/5
        [HttpGet("ParagraphROMDetail/References/{sectionRomId}")]
        [ProducesResponseType(typeof(IEnumerable<ReferenceDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ReferenceDTO>>> ParagraphROMDetailReferences(Guid sectionRomId)
        {
            try
            {
                return StatusCode(200, await _entity.GetParagraphROMDetailReference(sectionRomId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //---------------------------------------------------
        // POST: api/LawModificationRequests/Statue/Approve/5
        [HttpPost("Statue/Approve/{modifyingStatueId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> ApproveStatueROM(Guid modifyingStatueId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveStatueRom(modifyingStatueId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Statue/Deny/5
        [HttpPost("Statue/Deny/{modifyingStatueId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> DenyStatueROM(Guid modifyingStatueId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenyStatueRom(modifyingStatueId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Section/Approve/5
        [HttpPost("Section/Approve/{modifyingSectionId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> ApproveSectionROM(Guid modifyingSectionId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveSectionRom(modifyingSectionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Section/Deny/5
        [HttpPost("Section/Deny/{modifyingSectionId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> DenySectionROM(Guid modifyingSectionId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenySectionRom(modifyingSectionId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Paragraph/Approve/5
        [HttpPost("Paragraph/Approve/{modifyingParagraphId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> ApproveParagraphROM(Guid modifyingParagraphId)
        {
            try
            {
                return StatusCode(200, await _entity.ApproveParagraphRom(modifyingParagraphId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Paragraph/Deny/5
        [HttpPost("Paragraph/Deny/{modifyingParagraphId}")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> DenyParagraphROM(Guid modifyingParagraphId, [FromBody] string deniedReason)
        {
            try
            {
                return StatusCode(200, await _entity.DenyParagraphRom(modifyingParagraphId, deniedReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------
        // POST: api/LawModificationRequests/Cancel/5
        [HttpPost("Cancel")]
        [ProducesResponseType(typeof(LawModificationRequest), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LawModificationRequest>> CancelLawRom([FromBody] Guid lawRomId)
        {
            try
            {
                return StatusCode(200, await _entity.CancelLawRom(lawRomId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
