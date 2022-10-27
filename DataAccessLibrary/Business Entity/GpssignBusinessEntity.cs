using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class GpssignBusinessEntity
    {
        private IUnitOfWork work;
        public GpssignBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Gpssign>> GetGpssignsAsync()
        {
            return (await work.Gpssigns.GetAllAsync())
                .Where(gpssign => !gpssign.IsDeleted);
        }
        public async Task<Gpssign> GetGpssignAsync(Guid id)
        {
            return (await work.Gpssigns.GetAllAsync())
                .Where(gpssign => !gpssign.IsDeleted && gpssign.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Gpssign> AddGpssign(Gpssign gpssign)
        {
            gpssign.Id = Guid.NewGuid();
            gpssign.IsDeleted = false;
            await work.Gpssigns.AddAsync(gpssign);
            await work.Save();
            return gpssign;
        }
        public async Task<Gpssign> UpdateGpssign(Gpssign gpssign)
        {
            work.Gpssigns.Update(gpssign);
            await work.Save();
            return gpssign;
        }
        public async Task RemoveGpssign(Guid id)
        {
            Gpssign gpssign = await work.Gpssigns.GetAsync(id);
            gpssign.IsDeleted = true;
            work.Gpssigns.Update(gpssign);
            await work.Save();
        }
    }
}
