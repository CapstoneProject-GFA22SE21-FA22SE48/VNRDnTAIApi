using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class TestResultBusinessEntity
    {
        private IUnitOfWork work;
        public TestResultBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<TestResult>> GetTestResultsAsync()
        {
            return (await work.TestResults.GetAllAsync())
                .Where(testResult => !testResult.IsDeleted);
        }
        public async Task<TestResult> GetTestResultAsync(Guid id)
        {
            return (await work.TestResults.GetAllAsync())
                .Where(testResult => !testResult.IsDeleted && testResult.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<TestResult> AddTestResult(TestResult testResult)
        {
            testResult.Id = Guid.NewGuid();
            testResult.IsDeleted = false;
            await work.TestResults.AddAsync(testResult);
            await work.Save();
            return testResult;
        }
        public async Task<TestResult> UpdateTestResult(TestResult testResult)
        {
            work.TestResults.Update(testResult);
            await work.Save();
            return testResult;
        }
        public async Task RemoveTestResult(Guid id)
        {
            TestResult testResult = await work.TestResults.GetAsync(id);
            testResult.IsDeleted = true;
            work.TestResults.Update(testResult);
            await work.Save();
        }
    }
}
