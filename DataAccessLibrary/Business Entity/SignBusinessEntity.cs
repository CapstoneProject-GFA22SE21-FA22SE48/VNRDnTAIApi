using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignBusinessEntity
    {
        private IUnitOfWork work;
        public SignBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Sign>> GetSignsAsync()
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted);
        }
        public async Task<Sign> GetSignAsync(Guid id)
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Sign> AddSign(Sign sign)
        {
            sign.Id = Guid.NewGuid();
            sign.IsDeleted = false;
            await work.Signs.AddAsync(sign);
            await work.Save();
            return sign;
        }
        public async Task<Sign> UpdateSign(Sign sign)
        {
            work.Signs.Update(sign);
            await work.Save();
            return sign;
        }
        public async Task RemoveSign(Guid id)
        {
            Sign sign = await work.Signs.GetAsync(id);
            sign.IsDeleted = true;
            work.Signs.Update(sign);
            await work.Save();
        }
    }
}
