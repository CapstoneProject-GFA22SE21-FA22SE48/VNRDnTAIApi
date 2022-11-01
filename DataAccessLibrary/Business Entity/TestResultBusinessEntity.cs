using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
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

        public async Task<IEnumerable<TestResult>> GetTestResultByUserId(Guid userId)
        {
            await work.TestResults.GetAllAsync();
            var trs = (await work.TestResults.GetAllAsync())
                .Where(testResult => !testResult.IsDeleted && testResult.UserId == userId).OrderByDescending(o => o.CreatedDate);
            foreach (var tr in trs)
            {
                tr.TestResultDetails = (await work.TestResultDetails.GetAllAsync()).Where(trd => !trd.IsDeleted && trd.TestResultId == tr.Id).ToList();
            }
            return trs.Take(10);
        }

        public async Task<IEnumerable<TestAttempDTO>> GetTestAttemptDTOs(Guid testResultId)
        {
            var res = new List<TestAttempDTO>();
            await work.TestResultDetails.GetAllAsync();
            await work.Answers.GetAllAsync();
            var tr = (await work.TestResults.GetAsync(testResultId));
            foreach (var trd in tr.TestResultDetails)
            {
                var i = new TestAttempDTO();
                i.imageUrl = (await work.Questions.GetAsync(trd.QuestionId)).ImageUrl;
                i.questionContent = (await work.Questions.GetAsync(trd.QuestionId)).Content;
                i.chosenAnswerContent = (await work.Answers.GetAsync((Guid)trd.AnswerId)).Description;
                i.correctAnswerContent = (await work.Questions.GetAsync(trd.QuestionId)).Answers.FirstOrDefault(a => a.IsCorrect).Description;
                i.isCorrect = i.chosenAnswerContent.Equals(i.correctAnswerContent);
                res.Add(i);
            }
            return res;
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
            testResult.CreatedDate = DateTime.Now;
            testResult.IsDeleted = false;
            foreach (var trd in testResult.TestResultDetails)
            {
                trd.IsDeleted = false;
            }
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

        public async Task<IEnumerable<dynamic>> GetIncorrectQuestionsOfTestResults(Guid userId, Guid testCategoryId)
        {
            List<TestResult> testResults = (await work.TestResults.GetAllAsync())
                .Where(tr => !tr.IsDeleted && tr.TestCategoryId == testCategoryId)
                .ToList();
            return testResults;
        }
    }
}
