using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted);
        }
        public async Task<Statue> GetStatueAsync(Guid id)
        {
            return (await work.Statues.GetAllAsync())
                .Where(statue => !statue.IsDeleted && statue.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Statue> AddStatue(Statue statue)
        {
            statue.Id = Guid.NewGuid();
            statue.IsDeleted = false;
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
