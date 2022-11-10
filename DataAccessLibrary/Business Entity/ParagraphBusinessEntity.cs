using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.SearchLaw;
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

        public async Task<SearchLawDTO> GetSearchParagraphDTOAsync(Guid id)
        {
            var res = new SearchLawDTO();
            await work.Statues.GetAllAsync();
            var allPars = (await work.Paragraphs.GetAllAsync(nameof(Paragraph.Section), nameof(Paragraph.ReferenceReferenceParagraphs)))
                .Where(paragraph => paragraph.Status == (int)Status.Active
                    && !paragraph.IsDeleted)
                .ToList();
            var paragraph = allPars
               .Where(paragraph => !paragraph.IsDeleted
                       && paragraph.Status == (int)Status.Active
                       && paragraph.Id.Equals(id))
               .FirstOrDefault();

            res.Name = paragraph.Section.Statue.Name + " " + paragraph.Section.Name + " " + paragraph.Name;
            res.StatueDesc = char.ToUpper(paragraph.Section.Statue.Description.Remove(0, 8)[0]) + paragraph.Section.Statue.Description.Remove(0, 8).Substring(1);
            res.ParagraphDesc = paragraph.Description;
            res.SectionDesc = paragraph.Section.Description;
            res.MaxPenalty = paragraph.Section.MaxPenalty.ToString();
            res.MinPenalty = paragraph.Section.MinPenalty.ToString();
            res.AdditionalPenalty = paragraph.AdditionalPenalty != null ? " " + paragraph.AdditionalPenalty.ToString().Replace(".", ". \\\n") : "";
            res.ReferenceParagraph = paragraph.ReferenceReferenceParagraphs != null ? allPars.Where(par => paragraph.ReferenceReferenceParagraphs.Any(rp => rp.ParagraphId == par.Id)).
                Select(p => new SearchParagraphDTO
                {
                    Id = p.Id,
                    AdditionalPenalty = p.AdditionalPenalty,
                    Description = p.Description,
                    IsDeleted = false,
                    MaxPenalty = paragraph.Section.MaxPenalty.ToString(),
                    MinPenalty = paragraph.Section.MinPenalty.ToString(),
                    Name = p.Name,
                    SectionId = p.SectionId,
                    Status = p.Status,
                }).ToList() : null;
            return res;
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

        //This new paragraph is created for ROM of update, delete, create 
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

            if (paragraphDTO.KeywordId != null)
            {
                KeywordParagraph keywordParagraph = new KeywordParagraph
                {
                    ParagraphId = paragraph.Id,
                    KeywordId = (Guid)paragraphDTO.KeywordId,
                    IsDeleted = false
                };
                await work.KeywordParagraphs.AddAsync(keywordParagraph);
            }
            await work.Save();
            return paragraphDTO;
        }
    }
}
