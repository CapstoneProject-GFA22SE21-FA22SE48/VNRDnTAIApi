using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class ParagraphBusinessEntity
    {
        private IUnitOfWork work;
        public ParagraphBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Paragraph>> GetParagraphsAsync()
        {
            return (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted && paragraph.Status == (int)Status.Active);
        }
        public async Task<Paragraph> GetParagraphAsync(Guid id)
        {
            return (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active
                        && paragraph.Id.Equals(id))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<ParagraphDTO>> GetParagraphsBySectionIdAsync(Guid sectionId)
        {
            IEnumerable<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active
                        && paragraph.SectionId == sectionId)
                .OrderBy(paragraph => paragraph.Name);

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

            List<ParagraphDTO> dto = new List<ParagraphDTO>();
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
                dto.Add(new ParagraphDTO
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
                });
            }

            return dto;
        }

        //This new paragraph is created for ROM of update, delete 
        public async Task<ParagraphDTO> AddParagraph(ParagraphDTO paragraphDTO)
        {
            paragraphDTO.Id = Guid.NewGuid();
            paragraphDTO.Status = (int)Status.Deactivated;

            //If the statue is for Delete ROM, then keep IsDeleted = true
            if (paragraphDTO.IsDeleted == true) { }
            else
            {
                paragraphDTO.IsDeleted = false;
            }

            //Convert from paragraphDTO to paragraph
            Paragraph paragraph = new Paragraph
            {
                Id = paragraphDTO.Id,
                SectionId = paragraphDTO.SectionId,
                Name = paragraphDTO.Name,
                Description = paragraphDTO.Description,
                Status = paragraphDTO.Status,
                AdditionalPenalty = paragraphDTO.AdditionalPenalty,
                IsDeleted = paragraphDTO.IsDeleted
            };

            // If the paragraph has reference paragraphs -> insert to Reference table
            if (paragraphDTO.ReferenceParagraphs != null && paragraphDTO.ReferenceParagraphs.Count > 0)
            {
                foreach (var referenceParagraph in paragraphDTO.ReferenceParagraphs)
                {
                    await work.References.AddAsync(new Reference
                    {
                        ParagraphId = paragraphDTO.Id,
                        ReferenceParagraphId = referenceParagraph.ReferenceParagraphId,
                        IsExcluded = referenceParagraph.ReferenceParagraphIsExcluded,
                    });
                }
            }

            await work.Paragraphs.AddAsync(paragraph);
            await work.Save();
            return paragraphDTO;
        }
        public async Task<Paragraph> UpdateParagraph(Paragraph paragraph)
        {
            work.Paragraphs.Update(paragraph);
            await work.Save();
            return paragraph;
        }
        public async Task RemoveParagraph(Guid id)
        {
            Paragraph paragraph = await work.Paragraphs.GetAsync(id);
            paragraph.IsDeleted = true;
            work.Paragraphs.Update(paragraph);
            await work.Save();
        }
    }
}
