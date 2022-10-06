using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<TestCategory> GetTestCategoryAsync(Guid id)
        {
            return (await work.TestCategories.GetAllAsync())
                .Where(testCategory => !testCategory.IsDeleted && testCategory.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<TestCategory> AddTestCategory(TestCategory testCategory)
        {
            testCategory.Id = Guid.NewGuid();
            testCategory.IsDeleted = false;
            await work.TestCategories.AddAsync(testCategory);
            await work.Save();
            return testCategory;
        }
        public async Task<TestCategory> UpdateTestCategory(TestCategory testCategory)
        {
            work.TestCategories.Update(testCategory);
            await work.Save();
            return testCategory;
        }
        public async Task RemoveTestCategory(Guid id)
        {
            TestCategory testCategory = await work.TestCategories.GetAsync(id);
            testCategory.IsDeleted = true;
            work.TestCategories.Update(testCategory);
            await work.Save();
        }
    }
}
