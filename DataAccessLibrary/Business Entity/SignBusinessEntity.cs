using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Where(sign => !sign.IsDeleted && sign.Status == (int)Status.Active);
        }
        public async Task<Sign> GetSignAsync(Guid id)
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Id.Equals(id) && sign.Status == (int)Status.Active)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<Sign>> GetSignsBySignCategoryIdAsync(Guid signCategoryId)
        {
            return (await work.Signs.GetAllAsync())
                .Where(sign => !sign.IsDeleted && sign.Status == (int)Status.Active && sign.SignCategoryId == signCategoryId);
        }

        public async Task<IEnumerable<Sign>> GetScribeAssignedSignsAsync(Guid scribeId)
        {
            IEnumerable<Sign> signs = from assignedSignCategory in
                                          (await work.AssignedSignCategories.GetAllAsync())
                                          .Where(asc => !asc.IsDeleted && asc.ScribeId == scribeId)
                                      join signCategory in (await work.SignCategories.GetAllAsync())
                                                                   .Where(sc => !sc.IsDeleted)
                                      on assignedSignCategory.SignCategoryId equals signCategory.Id
                                      join sign in (await work.Signs.GetAllAsync())
                                                                   .Where(s => !s.IsDeleted && s.Status == (int)Status.Active)
                                      on signCategory.Id equals sign.SignCategoryId
                                      select sign;
            foreach (Sign sign in signs)
            {
                sign.SignCategory.Signs = null;
                sign.SignCategory.AssignedSignCategories = null;
            }
            return signs;
        }

        //This new sign is created for ROM of update, delete 
        public async Task<Sign> AddSignForROM(Sign sign)
        {
            sign.SignCategory = null;
            sign.Id = Guid.NewGuid();
            sign.Status = (int)Status.Deactivated;

            //If the sign is for Delete ROM, then keep IsDeleted = true
            if (sign.IsDeleted == true) { }
            else
            {
                sign.IsDeleted = false;
            }

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
