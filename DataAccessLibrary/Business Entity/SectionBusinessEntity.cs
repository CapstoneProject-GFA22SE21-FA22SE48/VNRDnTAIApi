using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
