using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.ManageROM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class LawModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public LawModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<LawModificationRequest>> GetLawModificationRequestsAsync()
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(lawModificationRequest => !lawModificationRequest.IsDeleted);
        }

        public async Task<LawModificationRequest>
            GetLawModificationRequestByModifyingParagraphIdAsync(Guid modifyingParagraphId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifyingParagraphId.Equals(modifyingParagraphId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<LawModificationRequest>>
            GetLawModificationRequestsByModifiedParagraphIdAsync(Guid modifiedParagraphId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifiedParagraphId.Equals(modifiedParagraphId));
        }

        public async Task<IEnumerable<LawModificationRequest>>
            GetLawModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ScribeId.Equals(scribeId));
        }

        public async Task<LawModificationRequest>
            AddLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            lawModificationRequest.Id = Guid.NewGuid();
            lawModificationRequest.Status = (int)Status.Pending;
            lawModificationRequest.CreatedDate = DateTime.Now;
            lawModificationRequest.IsDeleted = false;
            await work.LawModificationRequests.AddAsync(lawModificationRequest);
            await work.Save();
            return lawModificationRequest;
        }

        public async Task<LawModificationRequest>
            UpdateLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            work.LawModificationRequests.Update(lawModificationRequest);
            await work.Save();
            return lawModificationRequest;
        }
        public async Task RemoveLawModificationRequestByParagraphId(Guid modifyingParagraphId)
        {
            LawModificationRequest lawModificationRequest =
                (await work.LawModificationRequests.GetAllAsync())
                .Where(lm => !lm.IsDeleted && lm.ModifyingParagraphId == modifyingParagraphId)
                .FirstOrDefault();
            lawModificationRequest.IsDeleted = true;
            work.LawModificationRequests.Update(lawModificationRequest);
            await work.Save();
        }

        //-----------------------------------------------------
        public async Task<AdminRomListDTO> GetAdminRomList(Guid adminId)
        {
            //Load neccessary list
            IEnumerable<Statue> statues = (await work.Statues.GetAllAsync());
            IEnumerable<Section> sections = (await work.Sections.GetAllAsync());
            IEnumerable<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync());
            IEnumerable<Sign> signs = (await work.Signs.GetAllAsync());
            IEnumerable<Gpssign> gpssigns = (await work.Gpssigns.GetAllAsync());
            IEnumerable<User> users = (await work.Users.GetAllAsync());
            IEnumerable<Question> questions = (await work.Questions.GetAllAsync());

            // 1. Law Roms
            List<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.AdminId == adminId).ToList();
            List<LawRomDTO> lawRomDTOs = new List<LawRomDTO>();
            foreach (LawModificationRequest lawRom in lawRoms)
            {
                lawRomDTOs.Add(new LawRomDTO
                {
                    LawRomId = lawRom.Id,
                    ModifyingStatueId = (lawRom.ModifyingStatueId != null) ? lawRom.ModifyingStatueId : null,
                    ModifyingStatueName = (lawRom.ModifyingStatueId != null) ?
                    (statues.Where(s => s.Id == lawRom.ModifyingStatueId).FirstOrDefault().Name) : null,
                    ModifiedStatueId = (lawRom.ModifiedStatueId != null) ? lawRom.ModifiedStatueId : null,
                    ModifiedStatueName = (lawRom.ModifiedStatueId != null) ?
                    (statues.Where(s => s.Id == lawRom.ModifiedStatueId).FirstOrDefault().Name) : null,

                    ModifyingSectionId = (lawRom.ModifyingSectionId != null) ? lawRom.ModifyingSectionId : null,
                    ModifyingSectionName = (lawRom.ModifyingSectionId != null) ?
                    (sections.Where(s => s.Id == lawRom.ModifyingSectionId).FirstOrDefault().Name) : null,
                    ModifiedSectionId = (lawRom.ModifiedSectionId != null) ? lawRom.ModifiedSectionId : null,
                    ModifiedSectionName = (lawRom.ModifiedSectionId != null) ?
                    (sections.Where(s => s.Id == lawRom.ModifiedSectionId).FirstOrDefault().Name) : null,

                    ModifyingParagraphId = (lawRom.ModifyingParagraphId != null) ? lawRom.ModifyingParagraphId : null,
                    ModifyingParagraphName = (lawRom.ModifyingParagraphId != null) ?
                    (paragraphs.Where(p => p.Id == lawRom.ModifyingParagraphId).FirstOrDefault().Name) : null,
                    ModifiedParagraphId = (lawRom.ModifiedParagraphId != null) ? lawRom.ModifiedParagraphId : null,
                    ModifiedParagraphName = (lawRom.ModifiedParagraphId != null) ?
                    (paragraphs.Where(p => p.Id == lawRom.ModifiedParagraphId).FirstOrDefault().Name) : null,

                    ScribeId = lawRom.ScribeId,
                    Username = lawRom.Scribe != null ? lawRom.Scribe.Username :
                    (users.Where(u => u.Id == lawRom.ScribeId).FirstOrDefault().Username),
                    OperationType = lawRom.OperationType,
                    Status = lawRom.Status,
                    CreatedDate = lawRom.CreatedDate,
                    DeniedReason = lawRom.DeniedReason,
                });
            }


            // 2. Sign Roms
            List<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => !s.IsDeleted && s.AdminId == adminId
                && s.ScribeId != null & s.UserId == null).ToList();  //admin only handle ROM from scribe, ROM from handle will be handled by scribe

            List<SignRomDTO> signRomDTOs = new List<SignRomDTO>();
            foreach (SignModificationRequest signRom in signRoms)
            {
                signRomDTOs.Add(new SignRomDTO
                {
                    SignRomId = signRom.Id,
                    ModifyingSignId = signRom.ModifyingSignId != null ? signRom.ModifyingSignId : null,
                    ModifyingSignName = signRom.ModifyingSignId != null ?
                    (signs.Where(s => s.Id == signRom.ModifyingSignId).FirstOrDefault()).Name : null,
                    ModifiedSignId = signRom.ModifiedSignId != null ? signRom.ModifiedSignId : null,
                    ModifiedSignName = signRom.ModifiedSignId != null ?
                    (signs.Where(s => s.Id == signRom.ModifiedSignId).FirstOrDefault()).Name : null,

                    ModifyingGpssignId = signRom.ModifyingGpssignId != null ? signRom.ModifyingGpssignId : null,
                    ModifyingGpssignName = signRom.ModifyingGpssign != null ?
                    (signs.Where(s => s.Id == (gpssigns.Where(g => g.Id == signRom.ModifyingGpssignId).FirstOrDefault().SignId)))
                    .FirstOrDefault().Name : null,
                    ModifiedGpssignId = signRom.ModifiedGpssignId != null ? signRom.ModifiedGpssignId : null,
                    ModifiedGpssignName = signRom.ModifiedGpssign != null ?
                    (signs.Where(s => s.Id == (gpssigns.Where(g => g.Id == signRom.ModifiedGpssignId).FirstOrDefault().SignId)))
                    .FirstOrDefault().Name : null,

                    //admin only handle ROM from scribe, ROM from handle will be handled by scribe
                    //UserId = signRom.UserId != null ? signRom.UserId : null,
                    ScribeId = signRom.ScribeId != null ? signRom.ScribeId : null,
                    Username = signRom.UserId != null ?
                    (users.Where(u => u.Id == signRom.UserId).FirstOrDefault().Username) :
                    (users.Where(u => u.Id == signRom.ScribeId).FirstOrDefault().Username),
                    OperationType = signRom.OperationType,
                    Status = signRom.Status,
                    CreatedDate = signRom.CreatedDate,
                    DeniedReason = signRom.DeniedReason,
                });
            }


            //3. Question Roms
            List<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.AdminId == adminId).ToList();

            List<QuestionRomDTO> questionRomDTOs = new List<QuestionRomDTO>();
            foreach (QuestionModificationRequest questionRom in questionRoms)
            {
                questionRomDTOs.Add(new QuestionRomDTO
                {
                    ModifyingQuestionId = questionRom.ModifyingQuestionId,
                    ModifyingQuestionContent = questions.Where(q => q.Id == questionRom.ModifyingQuestionId).FirstOrDefault().Content,
                    ModifiedQuestionId = questionRom.ModifiedQuestionId != null ? questionRom.ModifiedQuestionId : null,
                    ModifiedQuestionContent = questionRom.ModifiedQuestionId != null ?
                    questions.Where(q => q.Id == questionRom.ModifiedQuestionId).FirstOrDefault().Content : null,

                    ScribeId = questionRom.ScribeId,
                    Username = questionRom.Scribe != null ? questionRom.Scribe.Username :
                    (users.Where(u => u.Id == questionRom.ScribeId).FirstOrDefault().Username),
                    OperationType = questionRom.OperationType,
                    Status = questionRom.Status,
                    CreatedDate = questionRom.CreatedDate,
                    DeniedReason = questionRom.DeniedReason
                });
            }

            //4. User Roms
            List<UserModificationRequest> userRoms = (await work.UserModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.ArbitratingAdminId == adminId).ToList();

            List<UserRomDTO> userRomDTOs = new List<UserRomDTO>();
            foreach (UserModificationRequest userRom in userRoms)
            {
                userRomDTOs.Add(new UserRomDTO
                {
                    ModifyingUserId = userRom.ModifyingUserId,
                    ModfifyingUserName = userRom.ModifyingUser != null ?
                    userRom.ModifyingUser.Username : users.Where(u => u.Id == userRom.ModifyingUserId).FirstOrDefault().Username,

                    PromotingAdminId = userRom.PromotingAdminId,
                    PromotingAdminUsername = userRom.PromotingAdmin != null ?
                    userRom.PromotingAdmin.Username : users.Where(u => u.Id == userRom.PromotingAdminId).FirstOrDefault().Username,
                    Status = userRom.Status,
                    CreatedDate = userRom.CreatedDate,
                    DeniedReason = userRom.DeniedReason
                });
            }

            AdminRomListDTO adminRomListDTO = new AdminRomListDTO
            {
                LawRoms = lawRomDTOs,
                SignRoms = signRomDTOs,
                QuestionRoms = questionRomDTOs,
                UserRoms = userRomDTOs
            };

            return adminRomListDTO;
        }

        public async Task<LawModificationRequest> GetLawRomDetail(Guid lawRomId)
        {
            LawModificationRequest lawRom = (await work.LawModificationRequests.GetAsync(lawRomId));

            if (lawRom.ModifyingStatueId != null)
            {
                lawRom.ModifyingStatue = (await work.Statues.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifyingStatueId).FirstOrDefault();

                if (lawRom.ModifiedStatueId != null)
                {
                    lawRom.ModifiedStatue = (await work.Statues.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifiedStatueId).FirstOrDefault();
                }
            }
            else if (lawRom.ModifyingSectionId != null)
            {
                IEnumerable<VehicleCategory> vehicleCategories = (await work.VehicleCategories.GetAllAsync());

                lawRom.ModifyingSection = (await work.Sections.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifyingSectionId).FirstOrDefault();

                lawRom.ModifyingSection.VehicleCategory = vehicleCategories
                    .Where(v => v.Id == lawRom.ModifyingSection.VehicleCategoryId).FirstOrDefault();
                lawRom.ModifyingSection.VehicleCategory.Sections = null;

                //check if section rom create a section with paragraphs
                List<Paragraph> sectionParagraphs = (await work.Paragraphs.GetAllAsync())
                    .Where(p => p.SectionId == lawRom.ModifyingSectionId).ToList();
                if (sectionParagraphs != null && sectionParagraphs.Count() > 0)
                {
                    lawRom.ModifyingSection.Paragraphs = sectionParagraphs;
                }

                if (lawRom.ModifiedSectionId != null)
                {
                    lawRom.ModifiedSection = (await work.Sections.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifiedSectionId).FirstOrDefault();

                    lawRom.ModifiedSection.VehicleCategory = vehicleCategories
                    .Where(v => v.Id == lawRom.ModifiedSection.VehicleCategoryId).FirstOrDefault();
                    lawRom.ModifyingSection.VehicleCategory.Sections = null;
                }
            }
            else if (lawRom.ModifyingParagraphId != null)
            {
                lawRom.ModifyingParagraph = (await work.Paragraphs.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifyingParagraphId).FirstOrDefault();

                //lawRom = (await GetParagraphROMDetailReference(lawRom.ModifyingParagraphId));

                if (lawRom.ModifiedParagraphId != null)
                {
                    lawRom.ModifiedParagraph = (await work.Paragraphs.GetAllAsync())
                    .Where(s => s.Id == lawRom.ModifiedParagraphId).FirstOrDefault();

                    //lawRom.ModifiedParagraphReferences = (await GetParagraphROMDetailReference(lawRom.ModifiedParagraphId)).ReferenceParagraphs;
                }
            }
            return lawRom;
        }

        public async Task<IEnumerable<ReferenceDTO>> GetParagraphROMDetailReference(Guid paragraphId)
        {
            IEnumerable<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync())
                .Where(p => p.Id == paragraphId);
            var tmpData1 = paragraphs.Join(
                    (await work.References.GetAllAsync()),
                    paragraph => paragraph.Id,
                    reference => reference.ParagraphId,
                    (paragraph, reference) => new
                    {
                        Id = paragraph.Id,
                        SectionId = paragraph.SectionId,
                        Name = paragraph.Name,
                        Description = paragraph.Description,
                        Status = paragraph.Status,
                        AdditionalPenalty = paragraph.AdditionalPenalty,
                        IsDeleted = paragraph.IsDeleted,
                        ReferenceParagraphId = reference.ReferenceParagraphId,
                        ReferenceParagraphIsExcluded = reference.IsExcluded
                    }
                    );

            //Paragraph with reference
            var tmpData2 = tmpData1.Join(
                (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active),
                tmp1 => tmp1.ReferenceParagraphId,
                paragraph => paragraph.Id,
                (tmp1, paragraph) => new
                {
                    Id = tmp1.Id,
                    SectionId = tmp1.SectionId,
                    Name = tmp1.Name,
                    Description = tmp1.Description,
                    Status = tmp1.Status,
                    AdditionalPenalty = tmp1.AdditionalPenalty,
                    IsDeleted = tmp1.IsDeleted,

                    ReferenceParagraphId = tmp1.ReferenceParagraphId,
                    ReferenceParagraphName = paragraph.Name,
                    ReferenceParagraphDesc = paragraph.Description,
                    ReferenceParagraphIsExcluded = tmp1.ReferenceParagraphIsExcluded,

                    ReferenceParagraphSectionId = paragraph.SectionId
                }
            );

            //With section
            var tmpData3 = tmpData2.Join(
                (await work.Sections.GetAllAsync())
                .Where(sc => !sc.IsDeleted && sc.Status == (int)Status.Active),
                tmp2 => tmp2.ReferenceParagraphSectionId,
                section => section.Id,
                (tmp2, section) => new
                {
                    Id = tmp2.Id,
                    SectionId = tmp2.SectionId,
                    Name = tmp2.Name,
                    Description = tmp2.Description,
                    Status = tmp2.Status,
                    AdditionalPenalty = tmp2.AdditionalPenalty,
                    IsDeleted = tmp2.IsDeleted,

                    ReferenceParagraphId = tmp2.ReferenceParagraphId,
                    ReferenceParagraphName = tmp2.ReferenceParagraphName,
                    ReferenceParagraphDesc = tmp2.ReferenceParagraphDesc,
                    ReferenceParagraphIsExcluded = tmp2.ReferenceParagraphIsExcluded,

                    ReferenceParagraphSectionId = tmp2.SectionId,
                    ReferenceParagraphSectionName = section.Name,

                    ReferenceParagraphSectionStatueId = section.StatueId
                }
                );

            //With statue
            var tmpData4 = tmpData3.Join(
                (await work.Statues.GetAllAsync())
                .Where(st => !st.IsDeleted && st.Status == (int)Status.Active),
                tmp3 => tmp3.ReferenceParagraphSectionStatueId,
                statue => statue.Id,
                (tmp3, statue) => new
                {
                    Id = tmp3.Id,
                    SectionId = tmp3.SectionId,
                    Name = tmp3.Name,
                    Description = tmp3.Description,
                    Status = tmp3.Status,
                    AdditionalPenalty = tmp3.AdditionalPenalty,
                    IsDeleted = tmp3.IsDeleted,

                    ReferenceParagraphId = tmp3.ReferenceParagraphId,
                    ReferenceParagraphName = tmp3.ReferenceParagraphName,
                    ReferenceParagraphDesc = tmp3.ReferenceParagraphDesc,
                    ReferenceParagraphIsExcluded = tmp3.ReferenceParagraphIsExcluded,

                    ReferenceParagraphSectionId = tmp3.SectionId,
                    ReferenceParagraphSectionName = tmp3.ReferenceParagraphSectionName,

                    ReferenceParagraphSectionStatueId = tmp3.ReferenceParagraphSectionStatueId,
                    ReferenceParagraphSectionStatueName = statue.Name
                }
                );

            ParagraphDTO paragraphDTO = null;
            List<ReferenceDTO> referenceList = null;

            foreach (var paragraph in paragraphs)
            {
                referenceList = new List<ReferenceDTO>();
                foreach (var data in tmpData4)
                {
                    if (data.Id == paragraph.Id)
                    {
                        referenceList.Add(new ReferenceDTO
                        {
                            ReferenceParagraphId = data.ReferenceParagraphId,
                            ReferenceParagraphName = data.ReferenceParagraphName,
                            ReferenceParagraphDesc = data.ReferenceParagraphDesc,

                            ReferenceParagraphSectionId = data.ReferenceParagraphSectionId,
                            ReferenceParagraphSectionName = data.ReferenceParagraphSectionName,

                            ReferenceParagraphSectionStatueId = data.ReferenceParagraphSectionStatueId,
                            ReferenceParagraphSectionStatueName = data.ReferenceParagraphSectionStatueName,
                            ReferenceParagraphIsExcluded = data.ReferenceParagraphIsExcluded,
                        });
                    }
                }
                paragraphDTO = new ParagraphDTO
                {
                    Id = paragraph.Id,
                    SectionId = paragraph.SectionId,
                    Name = paragraph.Name,
                    Description = paragraph.Description,
                    Status = paragraph.Status,
                    AdditionalPenalty = paragraph.AdditionalPenalty,
                    IsDeleted = paragraph.IsDeleted,

                    ReferenceParagraphs = referenceList.OrderBy(r => int.Parse(r.ReferenceParagraphSectionStatueName.Split(" ")[1]))
                    .ThenBy(r => int.Parse(r.ReferenceParagraphSectionName.Split(" ")[1]))
                    .ThenBy(r => r.ReferenceParagraphName).ToList()
                };
            }

            return paragraphDTO.ReferenceParagraphs;
        }
    }
}
