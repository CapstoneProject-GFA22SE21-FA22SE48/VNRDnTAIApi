using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.AdminReport;
using DTOsLibrary.ManageROM;
using Microsoft.EntityFrameworkCore;
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

        public async Task<LawModificationRequest>
            AddLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            //check if scribe is still in charge of this statue or section or paragraph
            if (lawModificationRequest.ModifyingStatueId != null)
            {
                IEnumerable<AssignedColumn> assignedColumns =
                    (await work.AssignedColumns.GetAllMultiIncludeAsync(
                        include: a => a
                        .Include(a => a.Column)
                        .ThenInclude(c => c.Statues)
                        ))
                    .Where(a => !a.IsDeleted && a.ScribeId == lawModificationRequest.ScribeId);
                bool isStillIncharge = false;
                if (assignedColumns != null)
                {
                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        if (assignedColumn.Column != null)
                        {
                            if (assignedColumn.Column.Statues != null)
                            {
                                foreach (Statue statue in assignedColumn.Column.Statues)
                                {
                                    if (statue.Id == lawModificationRequest.ModifyingStatueId)
                                    {
                                        isStillIncharge = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!isStillIncharge)
                {
                    throw new Exception("Điều này không còn thuộc phạm vi quản lý của bạn");
                }
            }
            else if (lawModificationRequest.ModifyingSectionId != null)
            {
                IEnumerable<AssignedColumn> assignedColumns =
                    (await work.AssignedColumns.GetAllMultiIncludeAsync(
                        include: a => a
                        .Include(a => a.Column)
                        .ThenInclude(c => c.Statues)
                        .ThenInclude(s => s.Sections)
                        ))
                    .Where(a => !a.IsDeleted && a.ScribeId == lawModificationRequest.ScribeId);
                bool isStillIncharge = false;
                if (assignedColumns != null)
                {
                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        if (assignedColumn.Column != null)
                        {
                            if (assignedColumn.Column.Statues != null)
                            {
                                foreach (Statue statue in assignedColumn.Column.Statues)
                                {
                                    if (statue.Sections != null)
                                    {
                                        foreach (Section section in statue.Sections)
                                        {
                                            if (section.Id == lawModificationRequest.ModifyingSectionId)
                                            {
                                                isStillIncharge = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!isStillIncharge)
                {
                    throw new Exception("Khoản này không còn thuộc phạm vi quản lý của bạn");
                }
            }
            else if (lawModificationRequest.ModifyingParagraphId != null)
            {
                IEnumerable<AssignedColumn> assignedColumns =
                    (await work.AssignedColumns.GetAllMultiIncludeAsync(
                        include: a => a
                        .Include(a => a.Column)
                        .ThenInclude(c => c.Statues)
                        .ThenInclude(s => s.Sections)
                        .ThenInclude(s => s.Paragraphs)
                        ))
                    .Where(a => !a.IsDeleted && a.ScribeId == lawModificationRequest.ScribeId);
                bool isStillIncharge = false;
                if (assignedColumns != null)
                {
                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        if (assignedColumn.Column != null)
                        {
                            if (assignedColumn.Column.Statues != null)
                            {
                                foreach (Statue statue in assignedColumn.Column.Statues)
                                {
                                    if (statue.Sections != null)
                                    {
                                        foreach (Section section in statue.Sections)
                                        {
                                            if (section.Paragraphs != null)
                                            {
                                                foreach (Paragraph paragraph in section.Paragraphs)
                                                {
                                                    if (paragraph.Id == lawModificationRequest.ModifyingParagraphId)
                                                    {
                                                        isStillIncharge = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!isStillIncharge)
                {
                    throw new Exception("Điểm này không còn thuộc phạm vi quản lý của bạn");
                }
            }

            //check if modifiedSection/modifiedParagraph is still active
            if (lawModificationRequest.ModifiedSectionId != null)
            {
                Section modifiedSection = await work.Sections.GetAsync((Guid)lawModificationRequest.ModifiedSectionId);
                if (modifiedSection != null)
                {
                    if (modifiedSection.Status == (int)Status.Deactivated || modifiedSection.IsDeleted == true)
                    {
                        throw new Exception("Khoản này không còn khả dụng");
                    }
                }
            }
            else if (lawModificationRequest.ModifiedParagraphId != null)
            {
                Paragraph modifiedParagraph = await work.Paragraphs.GetAsync((Guid)lawModificationRequest.ModifiedParagraphId);
                if (modifiedParagraph != null)
                {
                    if (modifiedParagraph.Status == (int)Status.Deactivated || modifiedParagraph.IsDeleted == true)
                    {
                        throw new Exception("Điểm này không còn khả dụng");
                    }
                }
            }

            lawModificationRequest.Id = Guid.NewGuid();
            lawModificationRequest.Status = (int)Status.Pending;
            lawModificationRequest.CreatedDate = DateTime.Now.ToLocalTime();
            lawModificationRequest.IsDeleted = false;
            await work.LawModificationRequests.AddAsync(lawModificationRequest);
            await work.Save();

            //Get data in return for notification adding
            lawModificationRequest.Scribe = (await work.Users.GetAllAsync())
                .Where(u => u.Id == lawModificationRequest.ScribeId)
                .FirstOrDefault();
            return lawModificationRequest;
        }


        public async Task RemoveLawModificationRequest(Guid lawRomId)
        {
            LawModificationRequest lawModificationRequest =
                (await work.LawModificationRequests.GetAllAsync())
                .Where(lm => lm.Id == lawRomId)
                .FirstOrDefault();
            if (lawModificationRequest != null)
            {
                lawModificationRequest.IsDeleted = true;
                if (lawModificationRequest.ModifyingStatueId != null)
                {
                    Statue statue = await work.Statues.GetAsync((Guid)lawModificationRequest.ModifyingStatueId);
                    if (statue != null)
                    {
                        statue.IsDeleted = true;
                        work.Statues.Update(statue);
                    }
                }
                else if (lawModificationRequest.ModifyingSectionId != null)
                {
                    Section section = await work.Sections.GetAsync((Guid)lawModificationRequest.ModifyingSectionId);
                    if (section != null)
                    {
                        section.IsDeleted = true;
                        work.Sections.Update(section);
                    }
                }
                else if (lawModificationRequest.ModifyingParagraphId != null)
                {
                    Paragraph paragraph = await work.Paragraphs.GetAsync((Guid)lawModificationRequest.ModifyingParagraphId);
                    if (paragraph != null)
                    {
                        paragraph.IsDeleted = true;
                        work.Paragraphs.Update(paragraph);
                    }
                }
                work.LawModificationRequests.Update(lawModificationRequest);
            }

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
                .Where(s => !s.IsDeleted && s.AdminId == adminId && s.ModifyingSignId != null
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
                .Where(rom => !rom.IsDeleted && (rom.ArbitratingAdminId == adminId || rom.PromotingAdminId == adminId)).ToList();

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

            //5. GPS Sign Roms
            List<SignModificationRequest> gpssignRoms =
                (await work.SignModificationRequests.GetAllMultiIncludeAsync(
                    include: gpsSign => gpsSign
                    .Include(g => g.ModifyingGpssign)
                    .ThenInclude(m => m.Sign)
                    .Include(g => g.ModifiedGpssign)
                    .ThenInclude(m => m.Sign)
                    .Include(g => g.User)
                    .Include(g => g.Scribe))
                    )
                    .Where(
                        rom => !rom.IsDeleted
                        && rom.ModifyingGpssignId != null
                        && (rom.AdminId == adminId)
                    ).ToList();

            AdminRomListDTO adminRomListDTO = new AdminRomListDTO
            {
                LawRoms = lawRomDTOs,
                SignRoms = signRomDTOs,
                QuestionRoms = questionRomDTOs,
                UserRoms = userRomDTOs,
                GPSSignRoms = gpssignRoms
            };

            return adminRomListDTO;
        }
        //--------------------------------------------------
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
        //--------------------------------------------------
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

        //--------------------------------------------------
        public async Task<LawModificationRequest> ApproveStatueRom(Guid modifyingStatueId)
        {
            LawModificationRequest statueRom = (await work.LawModificationRequests.GetAllAsync())
                .Where(l => l.ModifyingStatueId == modifyingStatueId).FirstOrDefault();

            if (statueRom != null)
            {
                if (statueRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (statueRom != null)
            {
                Statue modifyingStatue = await work.Statues.GetAsync((Guid)statueRom.ModifyingStatueId);
                Statue modifiedStatue = null;

                List<Section> relatedSections = new List<Section>();
                List<Paragraph> relatedParagraphsOfRelatedSections =
                        new List<Paragraph>();

                if (statueRom.ModifiedStatueId != null)
                {
                    modifiedStatue = (await work.Statues.GetAsync((Guid)statueRom.ModifiedStatueId));

                    relatedSections = (await work.Sections.GetAllAsync())
                        .Where(s => !s.IsDeleted && s.StatueId == modifiedStatue.Id).ToList();

                    IEnumerable<Paragraph> allParagraphs = (await work.Paragraphs.GetAllAsync())
                        .Where(p => !p.IsDeleted);


                    foreach (Section section in relatedSections)
                    {
                        relatedParagraphsOfRelatedSections
                            .AddRange(allParagraphs.Where(p => p.SectionId == section.Id));
                    }
                }

                //Scribe cannot create ROM of add statue
                if (statueRom.OperationType == (int)OperationType.Update)
                {
                    statueRom.Status = (int)Status.Approved;
                    if (modifyingStatue != null)
                    {
                        modifyingStatue.Status = (int)Status.Active;
                    }
                    if (modifiedStatue != null)
                    {
                        modifiedStatue.IsDeleted = true;

                        //All section ref to modifiedStatue -> now ref to modifyingStatue
                        foreach (Section section in relatedSections)
                        {
                            section.StatueId = (Guid)statueRom.ModifyingStatueId;
                        }

                        //Reference all Pending Rom of the modifiedStatueId to the new modifyingStatueId
                        IEnumerable<LawModificationRequest> statueRomsRefModifiedStatue =
                            (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending
                                    && l.ModifiedStatueId == statueRom.ModifiedStatueId);
                        foreach (LawModificationRequest statueMod in statueRomsRefModifiedStatue)
                        {
                            statueMod.ModifiedStatueId = statueRom.ModifyingStatueId;
                        }
                    }
                }
                else if (statueRom.OperationType == (int)OperationType.Delete)
                {
                    statueRom.Status = (int)Status.Approved;
                    if (modifyingStatue != null)
                    {
                        modifyingStatue.Status = (int)Status.Active;
                    }
                    if (modifiedStatue != null)
                    {
                        modifiedStatue.IsDeleted = true;

                        //All PENDING law roms
                        IEnumerable<LawModificationRequest> allLawRoms = (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending && !l.IsDeleted);

                        //Will be used to contain all PENDING sectionRom, paragraphRom related to modifiedStatueId
                        List<LawModificationRequest> relatedlawRoms = new List<LawModificationRequest>();

                        //Set IsDeleted to all sections ref to modifiedStatueId, then all paragraph ref to each section
                        //And add all related pending ROM of statue, section, paragraph that related to modifiedStatueId to a list
                        relatedlawRoms.AddRange(allLawRoms.Where(l => l.ModifiedStatueId == statueRom.ModifiedStatueId));
                        foreach (Section section in relatedSections)
                        {
                            section.IsDeleted = true;

                            relatedlawRoms.AddRange(allLawRoms.Where(l => l.ModifiedSectionId == section.Id));
                        }
                        foreach (Paragraph paragraph in relatedParagraphsOfRelatedSections)
                        {
                            paragraph.IsDeleted = true;

                            relatedlawRoms.AddRange(allLawRoms.Where(l => l.ModifiedParagraphId == paragraph.Id));
                        }

                        //Set status -> confirmed for all pending ROM of statue that related to modifiedStatueId
                        //And pending ROM of sections that related to modifiedStatueId,
                        //And pending ROM of parargaphs related to each section
                        foreach (LawModificationRequest relatedlawRom in relatedlawRoms)
                        {
                            relatedlawRom.Status = (int)Status.Confirmed;
                        }

                    }
                }
            }
            await work.Save();

            //include in return to use in notification
            statueRom.Scribe = (await work.Users.GetAsync(statueRom.ScribeId));
            statueRom.Admin = (await work.Users.GetAsync(statueRom.AdminId));
            statueRom.ModifyingStatue = (await work.Statues.GetAsync((Guid)statueRom.ModifyingStatueId));
            return statueRom;
        }
        //--------------------------------------------------
        public async Task<LawModificationRequest> DenyStatueRom(Guid modifyingStatueId, string deniedReason)
        {
            LawModificationRequest statueRom = (await work.LawModificationRequests.GetAllAsync())
               .Where(l => l.ModifyingStatueId == modifyingStatueId).FirstOrDefault();

            if (statueRom != null)
            {
                if (statueRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (statueRom != null)
            {
                statueRom.Status = (int)Status.Denied;
                statueRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == statueRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == statueRom.ScribeId && s.Status == (int)Status.Denied && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == statueRom.ScribeId && s.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == statueRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == statueRom.ScribeId && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == statueRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User deactivatingScribe = await work.Users.GetAsync((Guid)statueRom.ScribeId);
                    deactivatingScribe.Status = (int)Status.Deactivated;

                    //Remove all assigned tasks of scribe
                    IEnumerable<AssignedColumn> assignedColumns =
                        (await work.AssignedColumns.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedQuestionCategory> assignedQuestionCategories =
                        (await work.AssignedQuestionCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedSignCategory> assignedSignCategories =
                        (await work.AssignedSignCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        work.AssignedColumns.Delete(assignedColumn);
                    }

                    foreach (AssignedSignCategory assignedSignCategory in assignedSignCategories)
                    {
                        work.AssignedSignCategories.Delete(assignedSignCategory);
                    }

                    foreach (AssignedQuestionCategory assignedQuestionCategory in assignedQuestionCategories)
                    {
                        work.AssignedQuestionCategories.Delete(assignedQuestionCategory);
                    }

                    //Release all GPSSign roms that are "claimed" by current scribe
                    IEnumerable<SignModificationRequest> claimedGpssignRoms =
                        (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingGpssignId != null
                        && rom.Status == (int)Status.Claimed);
                    if (claimedGpssignRoms != null)
                    {
                        foreach (SignModificationRequest gpssignRom in claimedGpssignRoms)
                        {
                            gpssignRom.Status = (int)Status.Pending;
                            gpssignRom.ScribeId = null;
                        }
                    }

                    //Hard delete all Roms of scribe
                    IEnumerable<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                                .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingSignId != null);
                    IEnumerable<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);

                    if (lawRoms != null)
                    {
                        foreach (LawModificationRequest lawRom in lawRoms)
                        {
                            work.LawModificationRequests.Delete(lawRom);
                        }
                    }

                    if (signRoms != null)
                    {
                        foreach (SignModificationRequest signRom in signRoms)
                        {
                            work.SignModificationRequests.Delete(signRom);
                        }
                    }

                    if (questionRoms != null)
                    {
                        foreach (QuestionModificationRequest questionRom in questionRoms)
                        {
                            work.QuestionModificationRequests.Delete(questionRom);
                        }
                    }
                }
            }

            await work.Save();

            //include in return to use in notification
            statueRom.Scribe = (await work.Users.GetAsync(statueRom.ScribeId));
            statueRom.Admin = (await work.Users.GetAsync(statueRom.AdminId));
            statueRom.ModifyingStatue = (await work.Statues.GetAsync((Guid)statueRom.ModifyingStatueId));
            return statueRom;
        }
        //--------------------------------------------------
        public async Task<LawModificationRequest> ApproveSectionRom(Guid modifyingSectionId)
        {
            LawModificationRequest sectionRom = (await work.LawModificationRequests.GetAllAsync())
                .Where(l => l.ModifyingSectionId == modifyingSectionId).FirstOrDefault();

            if (sectionRom != null)
            {
                if (sectionRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (sectionRom != null)
            {
                Section modifyingSection = await work.Sections.GetAsync((Guid)sectionRom.ModifyingSectionId);
                Section modifiedSection = null;

                List<Paragraph> relatedParagraphsOfModifiedSection =
                        new List<Paragraph>();

                if (sectionRom.ModifiedSectionId != null)
                {
                    modifiedSection = (await work.Sections.GetAsync((Guid)sectionRom.ModifiedSectionId));

                    relatedParagraphsOfModifiedSection
                        .AddRange((await work.Paragraphs.GetAllAsync())
                        .Where(p => !p.IsDeleted && p.SectionId == sectionRom.ModifiedSectionId));
                }

                if (sectionRom.OperationType == (int)OperationType.Add)
                {
                    sectionRom.Status = (int)Status.Approved;
                    if (modifyingSection != null)
                    {
                        modifyingSection.Status = (int)Status.Active;
                    }
                    IEnumerable<Paragraph> relatedParagraphsOfNewSection =
                        (await work.Paragraphs.GetAllAsync())
                        .Where(p => !p.IsDeleted && p.SectionId == modifyingSection.Id);
                    if (relatedParagraphsOfNewSection != null)
                    {
                        foreach (Paragraph paragraph in relatedParagraphsOfNewSection)
                        {
                            paragraph.Status = (int)Status.Active;
                        }
                    }
                }
                else if (sectionRom.OperationType == (int)OperationType.Update)
                {
                    sectionRom.Status = (int)Status.Approved;
                    if (modifyingSection != null)
                    {
                        modifyingSection.Status = (int)Status.Active;
                    }
                    if (modifiedSection != null)
                    {
                        modifiedSection.IsDeleted = true;

                        //All paragraph ref to modifiedSection -> now ref to modifyingSection
                        foreach (Paragraph paragraph in relatedParagraphsOfModifiedSection)
                        {
                            paragraph.SectionId = (Guid)sectionRom.ModifyingSectionId;
                        }

                        //Reference all Pending Rom of the modifiedSectionId to the new modifyingSectionId
                        IEnumerable<LawModificationRequest> sectionRomsRefModifiedSection =
                            (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending
                                    && l.ModifiedSectionId == sectionRom.ModifiedSectionId);
                        foreach (LawModificationRequest sectionMod in sectionRomsRefModifiedSection)
                        {
                            sectionMod.ModifiedSectionId = sectionRom.ModifyingSectionId;
                        }
                    }
                }
                else if (sectionRom.OperationType == (int)OperationType.Delete)
                {
                    sectionRom.Status = (int)Status.Approved;
                    if (modifyingSection != null)
                    {
                        modifyingSection.Status = (int)Status.Active;
                    }
                    if (modifiedSection != null)
                    {
                        modifiedSection.IsDeleted = true;

                        //All PENDING law roms
                        IEnumerable<LawModificationRequest> allLawRoms = (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending && !l.IsDeleted);

                        //Will be used to contain all PENDING sectionRom, paragraphRom related to modifiedStatueId
                        List<LawModificationRequest> relatedlawRoms = new List<LawModificationRequest>();

                        //Set IsDeleted to all paragraph ref to each modifiedSectionId
                        //And add all related pending ROM of section, paragraph that related to modifiedSectionId to a list
                        relatedlawRoms.AddRange(allLawRoms.Where(l => l.ModifiedSectionId == sectionRom.ModifiedSectionId));
                        foreach (Paragraph paragraph in relatedParagraphsOfModifiedSection)
                        {
                            paragraph.IsDeleted = true;

                            relatedlawRoms.AddRange(allLawRoms.Where(l => l.ModifiedParagraphId == paragraph.Id));
                        }

                        //Set status -> confirmed for all pending ROM of section that related to modifiedSectionId
                        //And pending ROM of parargaphs related to section
                        foreach (LawModificationRequest relatedlawRom in relatedlawRoms)
                        {
                            relatedlawRom.Status = (int)Status.Confirmed;
                        }

                        //Set all paragraph of current deleting section to deactivated
                        IEnumerable<Paragraph> relatedParagraphs = (await work.Paragraphs.GetAllAsync())
                            .Where(p => p.SectionId == modifiedSection.Id);
                        if (relatedParagraphs != null)
                        {
                            foreach(Paragraph para in relatedParagraphs)
                            {
                                para.Status = (int)Status.Deactivated;
                                para.IsDeleted = true;
                            }
                        }
                    }
                }
            }
            await work.Save();

            //include in return to use in notification
            sectionRom.Scribe = (await work.Users.GetAsync(sectionRom.ScribeId));
            sectionRom.Admin = (await work.Users.GetAsync(sectionRom.AdminId));
            sectionRom.ModifyingSection = (await work.Sections.GetAsync((Guid)sectionRom.ModifyingSectionId));
            return sectionRom;
        }
        //--------------------------------------------------
        public async Task<LawModificationRequest> DenySectionRom(Guid modifyingSectionId, string deniedReason)
        {
            LawModificationRequest sectionRom = (await work.LawModificationRequests.GetAllAsync())
               .Where(l => l.ModifyingSectionId == modifyingSectionId).FirstOrDefault();

            if (sectionRom != null)
            {
                if (sectionRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (sectionRom != null)
            {
                sectionRom.Status = (int)Status.Denied;
                sectionRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == sectionRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == sectionRom.ScribeId && s.Status == (int)Status.Denied && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == sectionRom.ScribeId && s.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == sectionRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == sectionRom.ScribeId && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == sectionRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User deactivatingScribe = await work.Users.GetAsync((Guid)sectionRom.ScribeId);
                    deactivatingScribe.Status = (int)Status.Deactivated;

                    //Remove all assigned tasks of scribe
                    IEnumerable<AssignedColumn> assignedColumns =
                        (await work.AssignedColumns.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedQuestionCategory> assignedQuestionCategories =
                        (await work.AssignedQuestionCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedSignCategory> assignedSignCategories =
                        (await work.AssignedSignCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        work.AssignedColumns.Delete(assignedColumn);
                    }

                    foreach (AssignedSignCategory assignedSignCategory in assignedSignCategories)
                    {
                        work.AssignedSignCategories.Delete(assignedSignCategory);
                    }

                    foreach (AssignedQuestionCategory assignedQuestionCategory in assignedQuestionCategories)
                    {
                        work.AssignedQuestionCategories.Delete(assignedQuestionCategory);
                    }


                    //Release all GPSSign roms that are claimed by current scribe
                    IEnumerable<SignModificationRequest> claimedGpssignRoms =
                        (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingGpssignId != null);
                    if (claimedGpssignRoms != null)
                    {
                        foreach (SignModificationRequest gpssignRom in claimedGpssignRoms)
                        {
                            gpssignRom.Status = (int)Status.Pending;
                            gpssignRom.ScribeId = null;
                        }
                    }

                    //Hard delete all Roms of scribe
                    IEnumerable<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                                .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);

                    if (lawRoms != null)
                    {
                        foreach (LawModificationRequest lawRom in lawRoms)
                        {
                            work.LawModificationRequests.Delete(lawRom);
                        }
                    }

                    if (signRoms != null)
                    {
                        foreach (SignModificationRequest signRom in signRoms)
                        {
                            work.SignModificationRequests.Delete(signRom);
                        }
                    }

                    if (questionRoms != null)
                    {
                        foreach (QuestionModificationRequest questionRom in questionRoms)
                        {
                            work.QuestionModificationRequests.Delete(questionRom);
                        }
                    }
                }
            }

            await work.Save();

            //include in return to use in notification
            sectionRom.Scribe = (await work.Users.GetAsync(sectionRom.ScribeId));
            sectionRom.Admin = (await work.Users.GetAsync(sectionRom.AdminId));
            sectionRom.ModifyingSection = (await work.Sections.GetAsync((Guid)sectionRom.ModifyingSectionId));
            return sectionRom;
        }
        //--------------------------------------------------
        public async Task<LawModificationRequest> ApproveParagraphRom(Guid modifyingParagraphId)
        {
            LawModificationRequest paraRom = (await work.LawModificationRequests.GetAllAsync())
                .Where(p => p.ModifyingParagraphId == modifyingParagraphId).FirstOrDefault();

            if (paraRom != null)
            {
                if (paraRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (paraRom != null)
            {
                Paragraph modifyingParagraph = await work.Paragraphs.GetAsync((Guid)paraRom.ModifyingParagraphId);

                //check if is a section with no paragraph -> delete empty paragraph && referenceParagraph
                Section modifiedSection = (await work.Sections.GetAllAsync(nameof(Section.Paragraphs)))
                    .Where(s => s.Id == modifyingParagraph.SectionId).FirstOrDefault();
                Paragraph emptyParagraph = modifiedSection.Paragraphs.ToList().Where(p => string.IsNullOrEmpty(p.Name)).FirstOrDefault();
                if(emptyParagraph != null)
                {
                    emptyParagraph.IsDeleted = true;
                    IEnumerable<Reference> references = (await work.References.GetAllAsync())
                        .Where(r => r.ParagraphId == emptyParagraph.Id);
                    foreach(Reference r in references)
                    {
                        work.References.Delete(r);
                    }
                }

                Paragraph modifiedPargraph = null;
                if (paraRom.ModifiedParagraphId != null)
                {
                    modifiedPargraph = await work.Paragraphs.GetAsync((Guid)paraRom.ModifiedParagraphId);
                }

                if (paraRom.OperationType == (int)OperationType.Add)
                {
                    paraRom.Status = (int)Status.Approved;
                    if (modifyingParagraph != null)
                    {
                        modifyingParagraph.Status = (int)Status.Active;
                    }
                }
                else if (paraRom.OperationType == (int)OperationType.Update)
                {
                    paraRom.Status = (int)Status.Approved;
                    if (modifyingParagraph != null)
                    {
                        modifyingParagraph.Status = (int)Status.Active;
                    }
                    if (modifiedPargraph != null)
                    {
                        modifiedPargraph.IsDeleted = true;

                        //Reference all Pending Rom of the modifiedParagraphId to the new modifyingParagraphId
                        IEnumerable<LawModificationRequest> paraRomsRefModifiedParagraph =
                            (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending
                                    && l.ModifiedParagraphId == modifiedPargraph.Id);
                        foreach (LawModificationRequest lawMod in paraRomsRefModifiedParagraph)
                        {
                            lawMod.ModifiedParagraphId = modifyingParagraph.Id;
                        }

                        //Reference all SignParagraph of ModifiedParagraphId to ModifyingParagraphId
                        IEnumerable<SignParagraph> modifiedParagraphIdSignParagraphs =
                            (await work.SignParagraphs.GetAllAsync())
                            .Where(sp => !sp.IsDeleted && sp.ParagraphId == modifiedPargraph.Id);

                        if (modifiedParagraphIdSignParagraphs != null)
                        {
                            foreach (SignParagraph signPara in modifiedParagraphIdSignParagraphs)
                            {
                                signPara.ParagraphId = modifyingParagraph.Id;
                            }
                        }
                    }
                }
                else if (paraRom.OperationType == (int)OperationType.Delete)
                {
                    paraRom.Status = (int)Status.Approved;
                    if (modifyingParagraph != null)
                    {
                        modifyingParagraph.Status = (int)Status.Active;
                    }
                    if (modifiedPargraph != null)
                    {
                        modifiedPargraph.IsDeleted = true;

                        //Set status of all Pending ROM reference to the modifiedParagraphId to Confirmed
                        IEnumerable<LawModificationRequest> paraRomsRefModifiedParagraph =
                            (await work.LawModificationRequests.GetAllAsync())
                            .Where(l => l.Status == (int)Status.Pending
                                    && l.ModifiedParagraphId == modifiedPargraph.Id);

                        foreach (LawModificationRequest lawMod in paraRomsRefModifiedParagraph)
                        {
                            lawMod.Status = (int)Status.Confirmed;
                        }
                    }
                }
            }
            await work.Save();

            //include in return to use in notification
            paraRom.Scribe = (await work.Users.GetAsync(paraRom.ScribeId));
            paraRom.Admin = (await work.Users.GetAsync(paraRom.AdminId));
            paraRom.ModifyingParagraph = (await work.Paragraphs.GetAsync((Guid)paraRom.ModifyingParagraphId));
            return paraRom;
        }
        //--------------------------------------------------
        public async Task<LawModificationRequest> DenyParagraphRom(Guid modifyingParagraphId, string deniedReason)
        {
            LawModificationRequest paraRom = (await work.LawModificationRequests.GetAllAsync())
               .Where(l => l.ModifyingParagraphId == modifyingParagraphId).FirstOrDefault();

            if (paraRom != null)
            {
                if (paraRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

            if (paraRom != null)
            {
                paraRom.Status = (int)Status.Denied;
                paraRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == paraRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == paraRom.ScribeId && s.Status == (int)Status.Denied && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == paraRom.ScribeId && s.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == paraRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == paraRom.ScribeId && s.OperationType != (int)OperationType.Retrain).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == paraRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User deactivatingScribe = await work.Users.GetAsync((Guid)paraRom.ScribeId);
                    deactivatingScribe.Status = (int)Status.Deactivated;

                    //Remove all assigned tasks of scribe
                    IEnumerable<AssignedColumn> assignedColumns =
                        (await work.AssignedColumns.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedQuestionCategory> assignedQuestionCategories =
                        (await work.AssignedQuestionCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedSignCategory> assignedSignCategories =
                        (await work.AssignedSignCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        work.AssignedColumns.Delete(assignedColumn);
                    }

                    foreach (AssignedSignCategory assignedSignCategory in assignedSignCategories)
                    {
                        work.AssignedSignCategories.Delete(assignedSignCategory);
                    }

                    foreach (AssignedQuestionCategory assignedQuestionCategory in assignedQuestionCategories)
                    {
                        work.AssignedQuestionCategories.Delete(assignedQuestionCategory);
                    }

                    //Release all GPSSign roms that are claimed by current scribe
                    IEnumerable<SignModificationRequest> claimedGpssignRoms =
                        (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingGpssignId != null);
                    if (claimedGpssignRoms != null)
                    {
                        foreach (SignModificationRequest gpssignRom in claimedGpssignRoms)
                        {
                            gpssignRom.Status = (int)Status.Pending;
                            gpssignRom.ScribeId = null;
                        }
                    }

                    //Hard delete all Roms of scribe
                    IEnumerable<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                                .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);

                    if (lawRoms != null)
                    {
                        foreach (LawModificationRequest lawRom in lawRoms)
                        {
                            work.LawModificationRequests.Delete(lawRom);
                        }
                    }

                    if (signRoms != null)
                    {
                        foreach (SignModificationRequest signRom in signRoms)
                        {
                            work.SignModificationRequests.Delete(signRom);
                        }
                    }

                    if (questionRoms != null)
                    {
                        foreach (QuestionModificationRequest questionRom in questionRoms)
                        {
                            work.QuestionModificationRequests.Delete(questionRom);
                        }
                    }
                }
            }

            await work.Save();

            //include in return to use in notification
            paraRom.Scribe = (await work.Users.GetAsync(paraRom.ScribeId));
            paraRom.Admin = (await work.Users.GetAsync(paraRom.AdminId));
            paraRom.ModifyingParagraph = (await work.Paragraphs.GetAsync((Guid)paraRom.ModifyingParagraphId));
            return paraRom;
        }
        //--------------------------------------------------
        public async Task<RomReportDTO> GetAdminRomReportDTO(int month, int year, Guid adminId)
        {
            IEnumerable<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                .Where(l => !l.IsDeleted && l.AdminId == adminId && l.CreatedDate.Month == month && l.CreatedDate.Year == year);
            IEnumerable<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                .Where(l => !l.IsDeleted && l.AdminId == adminId && l.CreatedDate.Month == month && l.CreatedDate.Year == year);
            IEnumerable<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                .Where(l => !l.IsDeleted && l.AdminId == adminId && l.CreatedDate.Month == month && l.CreatedDate.Year == year);
            IEnumerable<UserModificationRequest> userRoms = (await work.UserModificationRequests.GetAllAsync())
                .Where(l => !l.IsDeleted && l.ArbitratingAdminId == adminId && l.CreatedDate.Month == month && l.CreatedDate.Year == year);

            RomReportDTO romReportDTO = new RomReportDTO
            {
                TotalRomCount = lawRoms.Count() + signRoms.Count() + questionRoms.Count() + userRoms.Count(),
                PendingRomCount = lawRoms.Where(r => r.Status == (int)Status.Pending).Count()
                + signRoms.Where(r => r.Status == (int)Status.Pending).Count()
                + questionRoms.Where(r => r.Status == (int)Status.Pending).Count()
                + userRoms.Where(r => r.Status == (int)Status.Pending).Count(),

                ApprovedRomCount = lawRoms.Where(r => r.Status == (int)Status.Approved).Count()
                + signRoms.Where(r => r.Status == (int)Status.Approved).Count()
                + questionRoms.Where(r => r.Status == (int)Status.Approved).Count()
                + userRoms.Where(r => r.Status == (int)Status.Approved).Count(),

                DeniedRomCount = lawRoms.Where(r => r.Status == (int)Status.Denied).Count()
                + signRoms.Where(r => r.Status == (int)Status.Denied).Count()
                + questionRoms.Where(r => r.Status == (int)Status.Denied).Count()
                + userRoms.Where(r => r.Status == (int)Status.Denied).Count(),

                ConfirmedRomCount = lawRoms.Where(r => r.Status == (int)Status.Confirmed).Count()
                + signRoms.Where(r => r.Status == (int)Status.Confirmed).Count()
                + questionRoms.Where(r => r.Status == (int)Status.Confirmed).Count()
                + userRoms.Where(r => r.Status == (int)Status.Confirmed).Count(),

                CancelledRomCount = lawRoms.Where(r => r.Status == (int)Status.Cancelled).Count()
                + signRoms.Where(r => r.Status == (int)Status.Cancelled).Count()
                + questionRoms.Where(r => r.Status == (int)Status.Cancelled).Count()
                + userRoms.Where(r => r.Status == (int)Status.Cancelled).Count(),
            };

            return romReportDTO;
        }
        //-----------------------------------------------------
        public async Task<ScribeRomListDTO> GetScribeRomList(Guid ScribeId)
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
                .Where(rom => !rom.IsDeleted && rom.ScribeId == ScribeId).ToList();
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

                    //Used for scribe rom
                    AdminId = lawRom.AdminId,
                    AdminUsername = (users.Where(u => u.Id == lawRom.AdminId).FirstOrDefault().Username)
                });
            }


            // 2. Sign Roms
            List<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => !s.IsDeleted && s.ScribeId == ScribeId
                && s.ScribeId != null & s.UserId == null).ToList();  //Scribe only handle ROM from scribe, ROM from handle will be handled by scribe

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

                    //UserId = signRom.UserId != null ? signRom.UserId : null,
                    ScribeId = signRom.ScribeId != null ? signRom.ScribeId : null,
                    Username = signRom.UserId != null ?
                    (users.Where(u => u.Id == signRom.UserId).FirstOrDefault().Username) :
                    (users.Where(u => u.Id == signRom.ScribeId).FirstOrDefault().Username),
                    OperationType = signRom.OperationType,
                    Status = signRom.Status,
                    CreatedDate = signRom.CreatedDate,
                    DeniedReason = signRom.DeniedReason,

                    //Used for scribe rom
                    AdminId = (Guid)signRom.AdminId,
                    AdminUsername = (users.Where(u => u.Id == signRom.AdminId).FirstOrDefault().Username)
                });
            }


            //3. Question Roms
            List<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.ScribeId == ScribeId).ToList();

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
                    DeniedReason = questionRom.DeniedReason,

                    //Used for scribe rom
                    AdminId = questionRom.AdminId,
                    AdminUsername = (users.Where(u => u.Id == questionRom.AdminId).FirstOrDefault().Username)
                });
            }

            ScribeRomListDTO ScribeRomListDTO = new ScribeRomListDTO
            {
                LawRoms = lawRomDTOs,
                SignRoms = signRomDTOs,
                QuestionRoms = questionRomDTOs,
            };

            return ScribeRomListDTO;
        }
        //---------------------------------------------------
        public async Task<LawModificationRequest> CancelLawRom(Guid lawRomId)
        {
            LawModificationRequest lawRom = (await work.LawModificationRequests.GetAsync(lawRomId));

            if (lawRom != null)
            {
                if (lawRom.Status == (int)Status.Approved || lawRom.Status == (int)Status.Denied)
                {
                    throw new Exception("Yêu cầu đã được xử lý");
                }
            }

            if (lawRom != null)
            {
                lawRom.Status = (int)Status.Cancelled;
            }
            await work.Save();
            return lawRom;
        }
    }
}
