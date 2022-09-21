using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class DecreeBusinessEntity
    {
        private IUnitOfWork work;
        public DecreeBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Decree>> GetDecreesAsync()
        {
            return (await work.Decrees.GetAllAsync())
                .Where(decree => !decree.IsDeleted);
        }
        public async Task<Decree> GetDecreeAsync(Guid id)
        {
            return (await work.Decrees.GetAllAsync())
                .Where(decree => !decree.IsDeleted && decree.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Decree> AddDecree(Decree decree)
        {
            decree.Id = Guid.NewGuid();
            decree.IsDeleted = false;
            await work.Decrees.AddAsync(decree);
            await work.Save();
            return decree;
        }
        public async Task<Decree> UpdateDecree(Decree decree)
        {
            work.Decrees.Update(decree);
            await work.Save();
            return decree;
        }
        public async Task RemoveDecree(Guid id)
        {
            Decree decree = await work.Decrees.GetAsync(id);
            decree.IsDeleted = true;
            work.Decrees.Update(decree);
            await work.Save();
        }
    }
}
