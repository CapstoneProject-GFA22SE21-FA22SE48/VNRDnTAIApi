using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class TestResultDetailBusinessEntity
    {
        private IUnitOfWork work;
        public TestResultDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<TestResultDetail>> GetTestResultDetailsAsync()
        {
            return (await work.TestResultDetails.GetAllAsync())
                .Where(testResultDetail => !testResultDetail.IsDeleted);
        }
        public async Task<TestResultDetail> GetTestResultDetailAsync(Guid id)
        {
            return (await work.TestResultDetails.GetAllAsync())
                .Where(testResultDetail => !testResultDetail.IsDeleted && testResultDetail.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<TestResultDetail> AddTestResultDetail(TestResultDetail testResultDetail)
        {
            testResultDetail.Id = Guid.NewGuid();
            testResultDetail.IsDeleted = false;
            await work.TestResultDetails.AddAsync(testResultDetail);
            await work.Save();
            return testResultDetail;
        }
        public async Task<TestResultDetail> UpdateTestResultDetail(TestResultDetail testResultDetail)
        {
            work.TestResultDetails.Update(testResultDetail);
            await work.Save();
            return testResultDetail;
        }
        public async Task RemoveTestResultDetail(Guid id)
        {
            TestResultDetail testResultDetail = await work.TestResultDetails.GetAsync(id);
            testResultDetail.IsDeleted = true;
            work.TestResultDetails.Update(testResultDetail);
            await work.Save();
        }
    }
}
