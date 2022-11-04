using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public SignModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<SignModificationRequest>> GetSignModificationRequestsAsync()
        {
            return await work.SignModificationRequests.GetAllAsync();
        }

        public async Task<SignModificationRequest> GetSignModificationRequestByModifyingSignIdAsync(Guid modifyingSignId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ModifyingSignId.Equals(modifyingSignId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsByModifiedSignIdAsync(Guid modifiedSignId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ModifiedSignId.Equals(modifiedSignId));
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ScribeId.Equals(scribeId));
        }

        public async Task<SignModificationRequest> AddSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            signModificationRequest.Id = Guid.NewGuid();
            signModificationRequest.Status = (int)Status.Pending;
            signModificationRequest.CreatedDate = DateTime.Now;
            signModificationRequest.IsDeleted = false;
            await work.SignModificationRequests.AddAsync(signModificationRequest);
            await work.Save();

            //Get data in return for notification adding
            signModificationRequest.Scribe = (await work.Users.GetAllAsync())
                .Where(u => u.Id == signModificationRequest.ScribeId)
                .FirstOrDefault();
            return signModificationRequest;
        }
        public async Task<SignModificationRequest> UpdateSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            work.SignModificationRequests.Update(signModificationRequest);
            await work.Save();
            return signModificationRequest;
        }
        public async Task RemoveSignModificationRequest(Guid signRomId)
        {
            SignModificationRequest signModificationRequest = await work.SignModificationRequests.GetAsync(signRomId);
            if (signModificationRequest != null)
            {
                signModificationRequest.IsDeleted = true;
                work.SignModificationRequests.Update(signModificationRequest);

                if (signModificationRequest.ModifyingSignId != null)
                {
                    Sign sign = await work.Signs.GetAsync((Guid)signModificationRequest.ModifyingSignId);
                    if (sign != null)
                    {
                        sign.IsDeleted = true;
                        work.Signs.Update(sign);
                    }
                }
                else if (signModificationRequest.ModifyingGpssignId != null)
                {
                    Gpssign gpssign = await work.Gpssigns.GetAsync((Guid)signModificationRequest.ModifyingGpssignId);
                    if (gpssign != null)
                    {
                        gpssign.IsDeleted = true;
                        work.Gpssigns.Update(gpssign);
                    }
                }
            }

            await work.Save();
        }

        //--------------------------------------------------
        public async Task<SignModificationRequestDTO> GetSignRomDetail(Guid modifyingSignId)
        {
            SignModificationRequest signRom =
                (await work.SignModificationRequests.GetAllMultiIncludeAsync(
                    include: signRom => signRom
                    .Include(s => s.ModifyingSign)
                    .ThenInclude(s => s.SignCategory)
                    .Include(s => s.ModifiedSign)
                    .ThenInclude(s => s.SignCategory)
                    ))
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();
            SignModificationRequestDTO signRomDTO = new SignModificationRequestDTO
            {
                Id = signRom.Id,
                ModifyingSignId = signRom.ModifyingSignId,
                ModifiedSignId = signRom.ModifiedSignId,
                //ModifyingGpssignId
                //ModifiedGpssignId
                UserId = signRom.UserId,
                ScribeId = signRom.ScribeId,
                AdminId = signRom.AdminId,
                OperationType = signRom.OperationType,
                ImageUrl = signRom.ImageUrl,
                Status = signRom.Status,
                DeniedReason = signRom.DeniedReason,
                CreatedDate = signRom.CreatedDate,
                IsDeleted = signRom.IsDeleted,

                Admin = signRom.Admin,
                //ModifiedGpssign
                //ModifyingGpssign
            };

            if (signRom.ModifiedSign != null)
            {
                signRomDTO.ModifiedSign = new SignDTO
                {
                    Id = signRom.ModifiedSign.Id,
                    SignCategoryId = signRom.ModifiedSign.SignCategoryId,
                    Name = signRom.ModifiedSign.Name,
                    Description = signRom.ModifiedSign.Description,
                    ImageUrl = signRom.ModifiedSign.ImageUrl,
                    Status = signRom.ModifiedSign.Status,
                    IsDeleted = signRom.ModifiedSign.IsDeleted,
                    SignCategory = signRom.ModifiedSign.SignCategory,
                };

                var tmpData1 = new List<dynamic>();
                tmpData1.Add(signRomDTO.ModifiedSign);

                //With paragraph
                var tmpData2 = from tmp1 in tmpData1
                               join signParagraph in (await work.SignParagraphs.GetAllAsync())
                               on tmp1.Id equals signParagraph.SignId

                               join paragraph in (await work.Paragraphs.GetAllAsync())
                               .Where(p => !p.IsDeleted && p.Status == (int)Status.Active)
                               on signParagraph.ParagraphId equals paragraph.Id
                               select new
                               {
                                   Id = tmp1.Id,
                                   SignCategoryId = tmp1.SignCategoryId,
                                   Name = tmp1.Name,
                                   Description = tmp1.Description,
                                   ImageUrl = tmp1.ImageUrl,
                                   Status = tmp1.Status,
                                   IsDeleted = tmp1.IsDeleted,
                                   SignCategory = tmp1.SignCategory,

                                   SignParagraphParagraphId = paragraph.Id,
                                   SignParagraphParagraphName = paragraph.Name,
                                   SignParagraphParagraphDesc = paragraph.Description,

                                   SignParagraphSectionId = paragraph.SectionId
                               };
                //With section
                var tmpData3 = from tmp2 in tmpData2
                               join section in (await work.Sections.GetAllAsync())
                                .Where(sc => !sc.IsDeleted && sc.Status == (int)Status.Active)
                                on tmp2.SignParagraphSectionId equals section.Id
                               select new
                               {
                                   Id = tmp2.Id,
                                   SignCategoryId = tmp2.SignCategoryId,
                                   Name = tmp2.Name,
                                   Description = tmp2.Description,
                                   ImageUrl = tmp2.ImageUrl,
                                   Status = tmp2.Status,
                                   IsDeleted = tmp2.IsDeleted,
                                   SignCategory = tmp2.SignCategory,

                                   SignParagraphParagraphId = tmp2.SignParagraphParagraphId,
                                   SignParagraphParagraphName = tmp2.SignParagraphParagraphName,
                                   SignParagraphParagraphDesc = tmp2.SignParagraphParagraphDesc,

                                   SignParagraphSectionId = tmp2.SignParagraphSectionId,
                                   SignParagraphSectionName = section.Name,

                                   SignParagraphStatueId = section.StatueId
                               };

                //With statue
                var tmpData4 = from tmp3 in tmpData3
                               join statue in (await work.Statues.GetAllAsync())
                                .Where(st => !st.IsDeleted && st.Status == (int)Status.Active)
                                on tmp3.SignParagraphStatueId equals statue.Id
                               select new
                               {
                                   Id = tmp3.Id,
                                   SignCategoryId = tmp3.SignCategoryId,
                                   Name = tmp3.Name,
                                   Description = tmp3.Description,
                                   ImageUrl = tmp3.ImageUrl,
                                   Status = tmp3.Status,
                                   IsDeleted = tmp3.IsDeleted,
                                   SignCategory = tmp3.SignCategory,

                                   SignParagraphParagraphId = tmp3.SignParagraphParagraphId,
                                   SignParagraphParagraphName = tmp3.SignParagraphParagraphName,
                                   SignParagraphParagraphDesc = tmp3.SignParagraphParagraphDesc,

                                   SignParagraphSectionId = tmp3.SignParagraphSectionId,
                                   SignParagraphSectionName = tmp3.SignParagraphSectionName,

                                   SignParagraphStatueId = tmp3.SignParagraphStatueId,
                                   SignParagraphStatueName = statue.Name
                               };
                List<SignParagraphDTO> signParagraphList = null;

                foreach (var sign in tmpData1)
                {
                    signParagraphList = new List<SignParagraphDTO>();
                    foreach (var data in tmpData4)
                    {
                        if (data.Id == sign.Id)
                        {
                            signParagraphList.Add(new SignParagraphDTO
                            {
                                SignParagraphParagraphId = data.SignParagraphParagraphId,
                                SignParagraphParagraphName = data.SignParagraphParagraphName,
                                SignParagraphParagraphDesc = data.SignParagraphParagraphDesc,

                                SignParagraphSectionId = data.SignParagraphSectionId,
                                SignParagraphSectionName = data.SignParagraphSectionName,

                                SignParagraphStatueId = data.SignParagraphStatueId,
                                SignParagraphStatueName = data.SignParagraphStatueName
                            });
                        }
                    }
                }
                signParagraphList.OrderBy(r => int.Parse(r.SignParagraphStatueName.Split(" ")[1]))
                        .ThenBy(r => int.Parse(r.SignParagraphSectionName.Split(" ")[1]))
                        .ThenBy(r => r.SignParagraphParagraphName).ToList();
                signRomDTO.ModifiedSign.SignParagraphs = signParagraphList;
            }

            if (signRom.ModifyingSign != null)
            {
                signRomDTO.ModifyingSign = new SignDTO
                {
                    Id = signRom.ModifyingSign.Id,
                    SignCategoryId = signRom.ModifyingSign.SignCategoryId,
                    Name = signRom.ModifyingSign.Name,
                    Description = signRom.ModifyingSign.Description,
                    ImageUrl = signRom.ModifyingSign.ImageUrl,
                    Status = signRom.ModifyingSign.Status,
                    IsDeleted = signRom.ModifyingSign.IsDeleted,
                    SignCategory = signRom.ModifyingSign.SignCategory,
                };

                var tmpData1 = new List<dynamic>();
                tmpData1.Add(signRomDTO.ModifyingSign);

                //With paragraph
                var tmpData2 = from tmp1 in tmpData1
                               join signParagraph in (await work.SignParagraphs.GetAllAsync())
                               on tmp1.Id equals signParagraph.SignId

                               join paragraph in (await work.Paragraphs.GetAllAsync())
                               .Where(p => !p.IsDeleted && p.Status == (int)Status.Active)
                               on signParagraph.ParagraphId equals paragraph.Id
                               select new
                               {
                                   Id = tmp1.Id,
                                   SignCategoryId = tmp1.SignCategoryId,
                                   Name = tmp1.Name,
                                   Description = tmp1.Description,
                                   ImageUrl = tmp1.ImageUrl,
                                   Status = tmp1.Status,
                                   IsDeleted = tmp1.IsDeleted,
                                   SignCategory = tmp1.SignCategory,

                                   SignParagraphParagraphId = paragraph.Id,
                                   SignParagraphParagraphName = paragraph.Name,
                                   SignParagraphParagraphDesc = paragraph.Description,

                                   SignParagraphSectionId = paragraph.SectionId
                               };
                //With section
                var tmpData3 = from tmp2 in tmpData2
                               join section in (await work.Sections.GetAllAsync())
                                .Where(sc => !sc.IsDeleted && sc.Status == (int)Status.Active)
                                on tmp2.SignParagraphSectionId equals section.Id
                               select new
                               {
                                   Id = tmp2.Id,
                                   SignCategoryId = tmp2.SignCategoryId,
                                   Name = tmp2.Name,
                                   Description = tmp2.Description,
                                   ImageUrl = tmp2.ImageUrl,
                                   Status = tmp2.Status,
                                   IsDeleted = tmp2.IsDeleted,
                                   SignCategory = tmp2.SignCategory,

                                   SignParagraphParagraphId = tmp2.SignParagraphParagraphId,
                                   SignParagraphParagraphName = tmp2.SignParagraphParagraphName,
                                   SignParagraphParagraphDesc = tmp2.SignParagraphParagraphDesc,

                                   SignParagraphSectionId = tmp2.SignParagraphSectionId,
                                   SignParagraphSectionName = section.Name,

                                   SignParagraphStatueId = section.StatueId
                               };

                //With statue
                var tmpData4 = from tmp3 in tmpData3
                               join statue in (await work.Statues.GetAllAsync())
                                .Where(st => !st.IsDeleted && st.Status == (int)Status.Active)
                                on tmp3.SignParagraphStatueId equals statue.Id
                               select new
                               {
                                   Id = tmp3.Id,
                                   SignCategoryId = tmp3.SignCategoryId,
                                   Name = tmp3.Name,
                                   Description = tmp3.Description,
                                   ImageUrl = tmp3.ImageUrl,
                                   Status = tmp3.Status,
                                   IsDeleted = tmp3.IsDeleted,
                                   SignCategory = tmp3.SignCategory,

                                   SignParagraphParagraphId = tmp3.SignParagraphParagraphId,
                                   SignParagraphParagraphName = tmp3.SignParagraphParagraphName,
                                   SignParagraphParagraphDesc = tmp3.SignParagraphParagraphDesc,

                                   SignParagraphSectionId = tmp3.SignParagraphSectionId,
                                   SignParagraphSectionName = tmp3.SignParagraphSectionName,

                                   SignParagraphStatueId = tmp3.SignParagraphStatueId,
                                   SignParagraphStatueName = statue.Name
                               };
                List<SignParagraphDTO> signParagraphList = null;

                foreach (var sign in tmpData1)
                {
                    signParagraphList = new List<SignParagraphDTO>();
                    foreach (var data in tmpData4)
                    {
                        if (data.Id == sign.Id)
                        {
                            signParagraphList.Add(new SignParagraphDTO
                            {
                                SignParagraphParagraphId = data.SignParagraphParagraphId,
                                SignParagraphParagraphName = data.SignParagraphParagraphName,
                                SignParagraphParagraphDesc = data.SignParagraphParagraphDesc,

                                SignParagraphSectionId = data.SignParagraphSectionId,
                                SignParagraphSectionName = data.SignParagraphSectionName,

                                SignParagraphStatueId = data.SignParagraphStatueId,
                                SignParagraphStatueName = data.SignParagraphStatueName
                            });
                        }
                    }
                }
                signParagraphList.OrderBy(r => int.Parse(r.SignParagraphStatueName.Split(" ")[1]))
                        .ThenBy(r => int.Parse(r.SignParagraphSectionName.Split(" ")[1]))
                        .ThenBy(r => r.SignParagraphParagraphName).ToList();
                signRomDTO.ModifyingSign.SignParagraphs = signParagraphList;
            }

            return signRomDTO;
        }
        //--------------------------------------------------
        public async Task<SignModificationRequest> GetGpssignRomDetail(Guid modifyingGpssignId)
        {
            SignModificationRequest gpssignRom =
                (await work.SignModificationRequests.GetAllAsync(nameof(SignModificationRequest.ModifyingGpssign), nameof(SignModificationRequest.ModifiedGpssign)))
                .Where(s => s.ModifyingGpssignId == modifyingGpssignId).FirstOrDefault();
            return gpssignRom;
        }
        //--------------------------------------------------
        public async Task<SignModificationRequest> ApproveSignRom(Guid modifyingSignId)
        {
            SignModificationRequest signRom = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();

            if (signRom != null)
            {
                Sign modifyingSign = await work.Signs.GetAsync((Guid)signRom.ModifyingSignId);
                Sign modifiedSign = null;
                if (signRom.ModifiedSignId != null)
                {
                    modifiedSign = await work.Signs.GetAsync((Guid)signRom.ModifiedSignId);
                }

                if (signRom.OperationType == (int)OperationType.Add)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                }
                else if (signRom.OperationType == (int)OperationType.Update)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                    if (modifiedSign != null)
                    {
                        modifiedSign.IsDeleted = true;

                        //Reference all Pending Rom of the modifiedSignId to the new modifyingSignId
                        IEnumerable<SignModificationRequest> signRomsRefModifiedSign =
                            (await work.SignModificationRequests.GetAllAsync())
                            .Where(s => s.Status == (int)Status.Pending
                                    && s.ModifiedSignId == modifiedSign.Id);
                        foreach (SignModificationRequest signMod in signRomsRefModifiedSign)
                        {
                            signMod.ModifiedSignId = modifyingSign.Id;
                        }
                    }
                }
                else if (signRom.OperationType == (int)OperationType.Delete)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                    if (modifiedSign != null)
                    {
                        modifiedSign.IsDeleted = true;

                        //Set status of all Pending ROM reference to the modifiedSignId to Confirmed
                        IEnumerable<SignModificationRequest> signRomsRefModifiedSign =
                            (await work.SignModificationRequests.GetAllAsync())
                            .Where(s => s.Status == (int)Status.Pending
                                    && s.ModifiedSignId == modifiedSign.Id);

                        foreach (SignModificationRequest signMod in signRomsRefModifiedSign)
                        {
                            signMod.Status = (int)Status.Confirmed;
                        }
                    }
                }
            }
            await work.Save();

            //include in return to use in notification
            if (signRom.ScribeId != null)
            {
                signRom.Scribe = (await work.Users.GetAsync((Guid)signRom.ScribeId));
            }
            if (signRom.AdminId != null)
            {
                signRom.Admin = (await work.Users.GetAsync((Guid)signRom.AdminId));
            }
            if (signRom.UserId != null)
            {
                signRom.User = (await work.Users.GetAsync((Guid)signRom.UserId));
            }
            if (signRom.ModifyingSignId != null)
            {
                signRom.ModifyingSign = (await work.Signs.GetAsync((Guid)signRom.ModifyingSignId));
            }
            if (signRom.ModifyingGpssignId != null)
            {
                signRom.ModifyingGpssign = (await work.Gpssigns.GetAsync((Guid)signRom.ModifyingGpssignId)); ;
            }
            return signRom;
        }
        //----------------------------------------------------
        public async Task<SignModificationRequest> DenySignRom(Guid modifyingSignId, string deniedReason)
        {
            SignModificationRequest signRom = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();
            if (signRom != null)
            {
                signRom.Status = (int)Status.Denied;
                signRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == signRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == signRom.ScribeId && s.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == signRom.ScribeId && s.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == signRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == signRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == signRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User scribe = await work.Users.GetAsync((Guid)signRom.ScribeId);
                    scribe.Status = (int)Status.Deactivated;

                    //All pending roms Status of scribe will be set as Confirmed
                    IEnumerable<LawModificationRequest> pendingLawRoms = (await work.LawModificationRequests.GetAllAsync())
                        .Where(rom => !rom.IsDeleted && rom.Status == (int)Status.Pending && rom.ScribeId == scribe.Id);
                    IEnumerable<SignModificationRequest> pendingSignRoms = (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => !rom.IsDeleted && rom.Status == (int)Status.Pending && rom.ScribeId == scribe.Id);
                    IEnumerable<QuestionModificationRequest> pendingQuestionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                        .Where(rom => !rom.IsDeleted && rom.Status == (int)Status.Pending && rom.ScribeId == scribe.Id);

                    if (pendingLawRoms != null)
                    {
                        foreach (LawModificationRequest pendingLawRom in pendingLawRoms)
                        {
                            pendingLawRom.Status = (int)Status.Confirmed;
                        }
                    }

                    if (pendingSignRoms != null)
                    {
                        foreach (SignModificationRequest pendingSignRom in pendingSignRoms)
                        {
                            pendingSignRom.Status = (int)Status.Confirmed;
                        }
                    }

                    if (pendingQuestionRoms != null)
                    {
                        foreach (QuestionModificationRequest pendingQuestionRom in pendingQuestionRoms)
                        {
                            pendingQuestionRom.Status = (int)Status.Confirmed;
                        }
                    }
                }
            }

            await work.Save();

            //include in return to use in notification
            if (signRom.ScribeId != null)
            {
                signRom.Scribe = (await work.Users.GetAsync((Guid)signRom.ScribeId));
            }
            if (signRom.AdminId != null)
            {
                signRom.Admin = (await work.Users.GetAsync((Guid)signRom.AdminId));
            }
            if (signRom.UserId != null)
            {
                signRom.User = (await work.Users.GetAsync((Guid)signRom.UserId));
            }
            if (signRom.ModifyingSignId != null)
            {
                signRom.ModifyingSign = (await work.Signs.GetAsync((Guid)signRom.ModifyingSignId));
            }
            if (signRom.ModifyingGpssignId != null)
            {
                signRom.ModifyingGpssign = (await work.Gpssigns.GetAsync((Guid)signRom.ModifyingGpssignId)); ;
            }
            return signRom;
        }
        //---------------------------------------------------
        public async Task<SignModificationRequest> CancelSignRom(Guid signRomId)
        {
            SignModificationRequest signRom = (await work.SignModificationRequests.GetAsync(signRomId));

            if (signRom != null)
            {
                signRom.Status = (int)Status.Cancelled;
            }
            await work.Save();
            return signRom;
        }
    }
}
