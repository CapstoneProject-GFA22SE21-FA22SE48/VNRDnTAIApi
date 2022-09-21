using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public SignCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<SignCategory>> GetSignCategoriesAsync()
        {
            return (await work.SignCategories.GetAllAsync())
                .Where(signCategory => !signCategory.IsDeleted);
        }
        public async Task<SignCategory> GetSignCategoryAsync(Guid id)
        {
            return (await work.SignCategories.GetAllAsync())
                .Where(signCategory => !signCategory.IsDeleted && signCategory.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<SignCategory> AddSignCategory(SignCategory signCategory)
        {
            signCategory.Id = Guid.NewGuid();
            signCategory.IsDeleted = false;
            await work.SignCategories.AddAsync(signCategory);
            await work.Save();
            return signCategory;
        }
        public async Task<SignCategory> UpdateSignCategory(SignCategory signCategory)
        {
            work.SignCategories.Update(signCategory);
            await work.Save();
            return signCategory;
        }
        public async Task RemoveSignCategory(Guid id)
        {
            SignCategory signCategory = await work.SignCategories.GetAsync(id);
            signCategory.IsDeleted = true;
            work.SignCategories.Update(signCategory);
            await work.Save();
        }
    }
}
