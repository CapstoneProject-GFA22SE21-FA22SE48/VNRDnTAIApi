using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class QuestionCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public QuestionCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<QuestionCategory>> GetQuestionCategoriesByTestCategoryId(Guid testCategoryId)
        {
            return (await work.QuestionCategories.GetAllAsync()).Where(q => !q.IsDeleted && q.TestCategoryId == testCategoryId);
        }
    }
}
