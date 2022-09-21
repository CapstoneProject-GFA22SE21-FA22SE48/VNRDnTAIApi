using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted);
        }
        public async Task<Section> GetSectionAsync(Guid id)
        {
            return (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted && section.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Section> AddSection(Section section)
        {
            section.Id = Guid.NewGuid();
            section.IsDeleted = false;
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
