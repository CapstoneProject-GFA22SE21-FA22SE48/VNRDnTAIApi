using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class TestCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public TestCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<TestCategory>> GetTestCategoriesAsync()
        {
            return (await work.TestCategories.GetAllAsync())
                .Where(testCategory => !testCategory.IsDeleted);
        }
        public async Task<IEnumerable<TestCategoryCountDTO>> CountQuestionsByTestCategoryId()
        {
            var data = from testCategory in (await work.TestCategories.GetAllAsync()).Where(e => !e.IsDeleted)
                       join question in (await work.Questions.GetAllAsync()).Where(q => !q.IsDeleted && q.Status == (int)Status.Active)
                       on testCategory.Id equals question.TestCategoryId
                       select new TestCategoryCountDTO
                       {
                           TestCategoryId = testCategory.Id,
                           TestCategoryName = testCategory.Name,
                       } into countData
                       group countData by new
                       {
                           countData.TestCategoryId,
                           countData.TestCategoryName
                       } into groupQ
                       select new TestCategoryCountDTO
                       {
                           TestCategoryId = groupQ.Key.TestCategoryId,
                           TestCategoryName = groupQ.Key.TestCategoryName,
                           QuestionsCount = groupQ.ToList().Count()
                       };
            return data.OrderBy(d => d.TestCategoryName);
        }
    }
}
