using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class StatueBusinessEntity
    {
        private IUnitOfWork work;
        public StatueBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Statue>> GetStatuesAsync()
        {
            IEnumerable<Statue> statues = (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted && statue.Status == (int)Status.Active);

            IEnumerable<Section> sections = (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted
                        && section.Status == (int)Status.Active);

            foreach (Statue statue in statues)
            {
                statue.Sections = sections.Where(section => section.StatueId == statue.Id).ToList();
            }

            return statues;
        }
        public async Task<Statue> GetStatueAsync(Guid id)
        {
            Statue statue = (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted
                        && statue.Status == (int)Status.Active
                        && statue.Id.Equals(id))
                .FirstOrDefault();

            List<Section> sections = (await work.Sections.GetAllAsync())
                .Where(section => !section.IsDeleted
                        && section.Status == (int)Status.Active
                        && section.StatueId == statue.Id).ToList();
            statue.Sections = sections;
            return statue;
        }

        //This new section is created for ROM of update, delete 
        public async Task<Statue> AddStatueForROM(Statue statue)
        {
            statue.Id = Guid.NewGuid();
            statue.Status = (int)Status.Deactivated;

            //If the statue is for Delete ROM, then keep IsDeleted = true
            if (statue.IsDeleted == true) { }
            else
            {
                statue.IsDeleted = false;
            }

            await work.Statues.AddAsync(statue);
            await work.Save();
            return statue;
        }
        public async Task<Statue> UpdateStatue(Statue statue)
        {
            work.Statues.Update(statue);
            await work.Save();
            return statue;
        }
        public async Task RemoveStatue(Guid id)
        {
            Statue statue = await work.Statues.GetAsync(id);
            statue.IsDeleted = true;
            work.Statues.Update(statue);
            await work.Save();
        }
    }
}
