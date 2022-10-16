using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class AssignedSignCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public AssignedSignCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<AssignedSignCategory>> GetAssignedSignCategoriesAsync()
        {
            return (await work.AssignedSignCategories.GetAllAsync())
                .Where(assignedSignCategory => !assignedSignCategory.IsDeleted);
        }
        public async Task<IEnumerable<AssignedSignCategory>> GetAssignedSignCategoriesByScribeIdAsync(Guid scribeId)
        {
            return (await work.AssignedSignCategories.GetAllAsync())
                .Where(assignedSignCategory => !assignedSignCategory.IsDeleted
                        && assignedSignCategory.ScribeId.Equals(scribeId));
        }
    }
}
