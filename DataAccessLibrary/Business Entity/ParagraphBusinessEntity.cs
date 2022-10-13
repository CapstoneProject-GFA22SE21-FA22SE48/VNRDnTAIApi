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

            var tmpTable1 = (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active
                        && paragraph.SectionId == sectionId)
                .OrderBy(paragraph => paragraph.Name).Join(
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
                        ReferenceParagraphId = reference.ReferenceParagraphId
                    }
                    );

            var tmpTable2 = tmpTable1.Join(
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
                }
            );

            List<ParagraphDTO> dto = new List<ParagraphDTO>();
            List<dynamic> referenceList = null;

            var tmpData = tmpTable2.GroupBy(p => p.Id).Select(p => p.First()).ToList();
            foreach (var paragraph in tmpData)
            {
                referenceList = new List<dynamic>();
                foreach (var data in tmpTable2)
                {
                    if (data.Id == paragraph.Id)
                    {
                        referenceList.Add(new
                        {
                            ReferenceParagraphId = data.ReferenceParagraphId,
                            ReferenceParagraphName = data.ReferenceParagraphName,
                            ReferenceParagraphDesc = data.ReferenceParagraphDesc
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

                    ReferenceParagraphs = referenceList
                });
            }

            return dto;
        }

        public async Task<Paragraph> AddParagraph(Paragraph paragraph)
        {
            paragraph.Id = Guid.NewGuid();
            paragraph.IsDeleted = false;
            await work.Paragraphs.AddAsync(paragraph);
            await work.Save();
            return paragraph;
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
