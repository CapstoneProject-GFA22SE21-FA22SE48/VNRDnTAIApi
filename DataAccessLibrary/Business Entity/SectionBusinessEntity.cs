using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using DTOsLibrary.CreateNewLaw;
using DTOsLibrary.SearchLaw;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static VNRDnTAILibrary.Utilities.StringUtils;

namespace DataAccessLibrary.Business_Entity
{
    public class SectionBusinessEntity
    {
        private IUnitOfWork work;
        public SectionBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Section>> GetSectionsAsync()
        {
            IEnumerable<Section> sections = (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted && section.Status == (int)Status.Active);

            List<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active).ToList();

            foreach (Section section in sections)
            {
                section.Paragraphs = paragraphs
                    .Where(paragraph => paragraph.SectionId == section.Id).ToList();
            }
            return sections;
        }

        public async Task<IEnumerable<Section>> GetSectionsByStatueIdAsync(Guid statueId)
        {
            IEnumerable<Section> sections =
                (await work.Sections.GetAllAsync(nameof(Section.VehicleCategory)))
                .Where(section => !section.IsDeleted
                        && section.Status == (int)Status.Active
                        && section.StatueId == statueId)
                .OrderBy(section => int.Parse(section.Name.Split(" ")[1]));

            List<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active).ToList();
            foreach (Section section in sections)
            {
                section.Paragraphs = paragraphs
                    .Where(paragraph => paragraph.SectionId == section.Id).ToList();
                section.VehicleCategory.Sections = null;
            }
            return sections;
        }

        public async Task<Section> GetSectionAsync(Guid id)
        {
            Section section = (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted && section.Id.Equals(id))
                .FirstOrDefault();

            List<Paragraph> paragraphs = (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted
                        && paragraph.Status == (int)Status.Active
                        && paragraph.SectionId == section.Id).ToList();
            section.Paragraphs = paragraphs;
            return section;
        }

        public async Task<IEnumerable<SearchLawDTO>> GetSearchListByQuery(string query, string vehicleCategory)
        {

            var vehicleCategoryId = (await work.VehicleCategories.GetAllAsync()).FirstOrDefault(vc => normaliseVietnamese(vehicleCategory) == normaliseVietnamese(vc.Name)).Id;
            query = normaliseVietnamese(query);
            var res = new List<SearchLawDTO>();
            await work.Statues.GetAllAsync();
            var allPars = (await work.Paragraphs.GetAllAsync(nameof(Paragraph.Section), nameof(Paragraph.ReferenceParagraphs)))
                .Where(paragraph => paragraph.Status == (int)Status.Active
                    && paragraph.Section.VehicleCategoryId == vehicleCategoryId
                    && !paragraph.IsDeleted)
                .ToList();
            //var paragraphList = allPars
            //    .Where(paragraph =>
            //    (normaliseVietnamese(paragraph.Description).Contains(query) || normaliseVietnamese(paragraph.Name).Contains(query)))
            //    .ToList();

            var paragraphList = (await work.Paragraphs.ExecuteQueryAsync(
                "select * FROM CONTAINSTABLE(Paragraph,Description, N\'\"" + query + "\"\') as r join Paragraph on Paragraph.Id = r.[KEY] ORDER BY RANK desc"))
                .Where(paragraph => paragraph.Status == (int)Status.Active
                    && paragraph.Section.VehicleCategoryId == vehicleCategoryId
                    && !paragraph.IsDeleted)
                .ToList();
            if(paragraphList.Count == 0)
            {
                paragraphList = (await work.Paragraphs.ExecuteQueryAsync(
                "select * FROM FREETEXTTABLE(Paragraph,Description, N\'\"" + query + "\"\') as r join Paragraph on Paragraph.Id = r.[KEY] ORDER BY RANK desc"))
                .Where(paragraph => paragraph.Status == (int)Status.Active
                    && paragraph.Section.VehicleCategoryId == vehicleCategoryId
                    && !paragraph.IsDeleted)
                .ToList();
            }

            foreach (var paragraph in paragraphList)
            {
                res.Add(new SearchLawDTO
                {
                    StatueDesc = char.ToUpper(paragraph.Section.Statue.Description.Remove(0, 8)[0]) + paragraph.Section.Statue.Description.Remove(0, 8).Substring(1),
                    ParagraphDesc = paragraph.Description.Replace(":", ":\\\n").Replace(";", ";\\\n \\\n"),
                    SectionDesc = paragraph.Section.Description,
                    MaxPenalty = paragraph.Section.MaxPenalty.ToString(),
                    MinPenalty = paragraph.Section.MinPenalty.ToString(),
                    AdditionalPenalty = paragraph.AdditionalPenalty != null ? " " + paragraph.AdditionalPenalty.ToString().Replace(".", ". \\\n") : "",
                    ReferenceParagraph = allPars.Where(par => paragraph.ReferenceReferenceParagraphs.Any(rp => rp.ParagraphId == par.Id)).
                    Select(p => new SearchParagraphDTO
                    {
                        Id = p.Id,
                        AdditionalPenalty = p.AdditionalPenalty,
                        Description = p.Description,
                        IsDeleted = false,
                        MaxPenalty = p.Section.MaxPenalty.ToString(),
                        MinPenalty = p.Section.MinPenalty.ToString(),
                        Name = p.Name,
                        SectionId = p.SectionId,
                        Status = p.Status,
                    })
                    .ToList()
                });
            }

            //var sectionsList = (await work.Sections.GetAllAsync())
            //.Where(section =>
            //(normaliseVietnamese(section.Description).Contains(query)
            //    || normaliseVietnamese(section.Name).Contains(query))
            //    && section.Status == (int)Status.Active
            //    && section.VehicleCategoryId == vehicleCategoryId
            //    && !section.IsDeleted)
            //.ToList();

            var sectionsList = (await work.Sections.ExecuteQueryAsync(
                "select * FROM CONTAINSTABLE(Section,Description, N\'\"" + query + "\"\') as r join Section on Section.Id = r.[KEY] ORDER BY RANK desc"))
            .Where(section =>
            (normaliseVietnamese(section.Description).Contains(query)
                || normaliseVietnamese(section.Name).Contains(query))
                && section.Status == (int)Status.Active
                && section.VehicleCategoryId == vehicleCategoryId
                && !section.IsDeleted)
            .ToList();

            foreach (var section in sectionsList)
            {
                res.Add(new SearchLawDTO
                {
                    StatueDesc = char.ToUpper(section.Statue.Description.Remove(0, 8)[0]) + section.Statue.Description.Remove(0, 8).Substring(1),
                    ParagraphDesc = "",
                    SectionDesc = section.Description,
                    MaxPenalty = section.MaxPenalty.ToString(),
                    MinPenalty = section.MinPenalty.ToString(),
                    AdditionalPenalty = section.Paragraphs.FirstOrDefault(p => p.SectionId == section.Id).AdditionalPenalty != null ? " " + section.Paragraphs.FirstOrDefault(p => p.SectionId == section.Id).AdditionalPenalty.ToString().Replace(".", ". \\\n") : "",
                    ReferenceParagraph = allPars.Where(par => section.Paragraphs.FirstOrDefault(p => p.SectionId == section.Id).ReferenceReferenceParagraphs.Any(rp => rp.ParagraphId == par.Id)).
                    Select(p => new SearchParagraphDTO
                    {
                        Id = p.Id,
                        AdditionalPenalty = p.AdditionalPenalty,
                        Description = p.Description,
                        IsDeleted = false,
                        MaxPenalty = p.Section.MaxPenalty.ToString(),
                        MinPenalty = p.Section.MinPenalty.ToString(),
                        Name = p.Name,
                        SectionId = p.SectionId,
                        Status = p.Status,
                    })
                    .ToList()
                });
            }

            return res;

        }


        public async Task<IEnumerable<SearchLawDTO>> GetSearchListByKeywordId(Guid keywordId, string vehicleCategory)
        {

            var vehicleCategoryId = (await work.VehicleCategories.GetAllAsync()).FirstOrDefault(vc => normaliseVietnamese(vehicleCategory) == normaliseVietnamese(vc.Name)).Id;
            var res = new List<SearchLawDTO>();
            await work.Statues.GetAllAsync();
            var allPars = (await work.Paragraphs.GetAllAsync(nameof(Paragraph.Section), nameof(Paragraph.ReferenceParagraphs)))
                .Where(paragraph => paragraph.Status == (int)Status.Active
                    && paragraph.Section.VehicleCategoryId == vehicleCategoryId
                    && !paragraph.IsDeleted)
                .ToList();

            var paragraphsInKeywordList = (await work.KeywordParagraphs.GetAllAsync()).Where(k => k.KeywordId == keywordId).ToList();

            var paragraphList = allPars
                .Where(paragraph =>
                  paragraphsInKeywordList.Any(pk => pk.ParagraphId == paragraph.Id)
                  && paragraph.Status == (int)Status.Active
                  && paragraph.Section.VehicleCategoryId == vehicleCategoryId
                  && !paragraph.IsDeleted)
                .ToList();


            foreach (var paragraph in paragraphList)
            {
                res.Add(new SearchLawDTO
                {
                    StatueDesc = char.ToUpper(paragraph.Section.Statue.Description.Remove(0, 8)[0]) + paragraph.Section.Statue.Description.Remove(0, 8).Substring(1),
                    ParagraphDesc = paragraph.Description,
                    SectionDesc = paragraph.Section.Description,
                    MaxPenalty = paragraph.Section.MaxPenalty.ToString(),
                    MinPenalty = paragraph.Section.MinPenalty.ToString(),
                    AdditionalPenalty = paragraph.AdditionalPenalty != null ? " " + paragraph.AdditionalPenalty.ToString().Replace(".", ". \\\n") : "",
                    ReferenceParagraph = allPars.Where(par => paragraph.ReferenceReferenceParagraphs.Any(rp => rp.ParagraphId == par.Id)).
                    Select(p => new SearchParagraphDTO
                    {
                        Id = p.Id,
                        AdditionalPenalty = p.AdditionalPenalty,
                        Description = p.Description,
                        IsDeleted = false,
                        MaxPenalty = p.Section.MaxPenalty.ToString(),
                        MinPenalty = p.Section.MinPenalty.ToString(),
                        Name = p.Name,
                        SectionId = p.SectionId,
                        Status = p.Status,
                    })
                    .ToList()
                });
            }
            return res;

        }

        //This new section is created for ROM of update, delete 
        public async Task<Section> AddSectionForROM(Section section)
        {
            section.Id = Guid.NewGuid();
            section.Status = (int)Status.Deactivated;

            //If the section is for Delete ROM, then keep IsDeleted = true
            if (section.IsDeleted == true) { }
            else
            {
                section.IsDeleted = false;
            }

            await work.Sections.AddAsync(section);
            await work.Save();
            return section;
        }

        //Used for creating a new section (with/without paragraphs)
        public async Task<Section> CreateNewSection(NewSectionDTO newSectionDTO)
        {
            Section newSection = new Section
            {
                Id = Guid.NewGuid(),
                Name = newSectionDTO.Name,
                VehicleCategoryId = newSectionDTO.VehicleCategoryId,
                StatueId = newSectionDTO.StatueId,
                Description = newSectionDTO.Description,
                MinPenalty = newSectionDTO.MinPenalty,
                MaxPenalty = newSectionDTO.MaxPenalty,
                Status = (int)Status.Deactivated,
                IsDeleted = false
            };

            await work.Sections.AddAsync(newSection);

            if (newSectionDTO.IsSectionWithNoParagraph)
            {
                Paragraph newParagraph = new Paragraph
                {
                    Id = Guid.NewGuid(),
                    SectionId = newSection.Id,
                    Name = "",
                    Description = "",
                    Status = (int)Status.Deactivated,
                    AdditionalPenalty = "",
                    IsDeleted = false
                };

                await work.Paragraphs.AddAsync(newParagraph);

                if (newSectionDTO.ReferenceParagraphs != null)
                {
                    foreach (ReferenceDTO referenceParagraph in newSectionDTO.ReferenceParagraphs)
                    {
                        Reference reference = new Reference
                        {
                            ParagraphId = newParagraph.Id,
                            ReferenceParagraphId = referenceParagraph.ReferenceParagraphId,
                            IsExcluded = referenceParagraph.ReferenceParagraphIsExcluded
                        };
                        await work.References.AddAsync(reference);
                    }
                }
            }
            else
            {
                if (newSectionDTO.Paragraphs != null)
                {
                    foreach (NewParagraphDTO newParagraphDTO in newSectionDTO.Paragraphs)
                    {
                        Paragraph paragraph = new Paragraph
                        {
                            Id = Guid.NewGuid(),
                            SectionId = newSection.Id,
                            Name = newParagraphDTO.Name,
                            Description = newParagraphDTO.Description,
                            Status = (int)Status.Deactivated,
                            AdditionalPenalty = newParagraphDTO.AdditionalPenalty,
                            IsDeleted = false
                        };
                        await work.Paragraphs.AddAsync(paragraph);

                        if (newParagraphDTO.KeywordId != null)
                        {
                            KeywordParagraph keywordParagraph = new KeywordParagraph
                            {
                                KeywordId = (Guid)newParagraphDTO.KeywordId,
                                ParagraphId = paragraph.Id,
                                IsDeleted = false
                            };
                            await work.KeywordParagraphs.AddAsync(keywordParagraph);
                        }

                        if (newParagraphDTO.ReferenceParagraphs != null)
                        {
                            foreach (ReferenceDTO referenceParagraph in newParagraphDTO.ReferenceParagraphs)
                            {
                                Reference reference = new Reference
                                {
                                    ParagraphId = paragraph.Id,
                                    ReferenceParagraphId = referenceParagraph.ReferenceParagraphId,
                                    IsExcluded = referenceParagraph.ReferenceParagraphIsExcluded
                                };
                                await work.References.AddAsync(reference);
                            }
                        }
                    }
                }
            }

            await work.Save();
            return newSection;
        }

        public async Task<Section> UpdateSection(Section section)
        {
            work.Sections.Update(section);
            await work.Save();
            return section;
        }
        public async Task RemoveSection(Guid id)
        {
            Section section = await work.Sections.GetAsync(id);
            section.IsDeleted = true;
            work.Sections.Update(section);
            await work.Save();
        }
    }
}
