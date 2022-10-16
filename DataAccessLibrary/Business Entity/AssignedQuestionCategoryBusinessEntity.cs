using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class AssignedQuestionCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public AssignedQuestionCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<AssignedQuestionCategory>> GetAssignedQuestionCategoriesAsync()
        {
            return (await work.AssignedQuestionCategories.GetAllAsync())
                .Where(assignedQuestionCategory => !assignedQuestionCategory.IsDeleted);
        }
        public async Task<IEnumerable<AssignedQuestionCategory>> GetAssignedQuestionCategoriesByScribeIdAsync(Guid scribeId)
        {
            return (await work.AssignedQuestionCategories.GetAllAsync())
                .Where(assignedQuestionCategory => !assignedQuestionCategory.IsDeleted
                        && assignedQuestionCategory.ScribeId.Equals(scribeId));
        }
    }
}
