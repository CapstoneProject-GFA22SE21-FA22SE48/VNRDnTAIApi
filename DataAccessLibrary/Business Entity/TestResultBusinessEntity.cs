using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<TestResult>> GetTestResultByUserId(Guid userId, Guid testCategoryId)
        {
            await work.TestResults.GetAllAsync();
            var trds = await work.TestResultDetails.GetAllAsync();
            var trs = (await work.TestResults.GetAllAsync())
                .Where(testResult => !testResult.IsDeleted && testResult.UserId == userId && testResult.TestCategoryId == testCategoryId).OrderByDescending(o => o.CreatedDate);
            foreach (var tr in trs)
            {
                tr.TestResultDetails = trds.Where(trd => !trd.IsDeleted && trd.TestResultId == tr.Id).ToList();
            }
            return trs;
        }

        public async Task<IEnumerable<TestAttempDTO>> GetTestAttemptDTOs(Guid testResultId, Guid userId, Guid testCategoryId)
        {
            //Get Wrong Answer
            var res = new List<TestAttempDTO>();
            await work.TestResultDetails.GetAllAsync();
            await work.Answers.GetAllAsync();

            if (testResultId == Guid.Empty)
            {
                var trs = (await work.TestResults.GetAllAsync())
                .Where(testResult => !testResult.IsDeleted && testResult.UserId == userId && testResult.TestCategoryId == testCategoryId).OrderByDescending(o => o.CreatedDate).Take(10);
                foreach (var tr in trs)
                {
                    tr.TestResultDetails = (await work.TestResultDetails.GetAllAsync()).Where(trd => !trd.IsDeleted && trd.TestResultId == tr.Id).ToList();
                }

                var onlyWrongTestResultDetail = new List<TestResultDetail>();
                var wrong = new List<TestResultDetail>();
                var right = new List<TestResultDetail>();

                foreach (var tr in trs)
                {
                    foreach (var trd in tr.TestResultDetails)
                    {
                        if (trd.IsCorrect)
                        {
                            right.Add(trd);
                        }
                        else
                        {
                            wrong.Add(trd);
                        }
                    }
                }
                onlyWrongTestResultDetail = wrong.Where(w => !right.Contains(w)).DistinctBy(x => x.QuestionId).ToList();
                foreach (var trd in onlyWrongTestResultDetail)
                {
                    if (trd.Answer != null)
                    {
                        var i = new TestAttempDTO();
                        i.imageUrl = (await work.Questions.GetAsync(trd.QuestionId)).ImageUrl;
                        i.questionContent = (await work.Questions.GetAsync(trd.QuestionId)).Content;
                        i.chosenAnswerContent = trd.Answer != null ? (await work.Answers.GetAsync((Guid)trd.AnswerId)).Description : "";
                        i.correctAnswerContent = (await work.Questions.GetAsync(trd.QuestionId)).Answers.FirstOrDefault(a => a.IsCorrect).Description;
                        i.isCorrect = i.chosenAnswerContent.Equals(i.correctAnswerContent);
                        res.Add(i);
                    }
                }
            }
            else
            {
                var tr = (await work.TestResults.GetAsync(testResultId));
                foreach (var trd in tr.TestResultDetails)
                {
                    var i = new TestAttempDTO();
                    i.imageUrl = (await work.Questions.GetAsync(trd.QuestionId)).ImageUrl;
                    i.questionContent = (await work.Questions.GetAsync(trd.QuestionId)).Content;
                    i.chosenAnswerContent = trd.Answer != null ? (await work.Answers.GetAsync((Guid)trd.AnswerId)).Description : "";
                    i.correctAnswerContent = (await work.Questions.GetAsync(trd.QuestionId)).Answers.FirstOrDefault(a => a.IsCorrect).Description;
                    i.isCorrect = i.chosenAnswerContent.Equals(i.correctAnswerContent);
                    res.Add(i);
                }
            }
            return res;
        }

        public async Task<TestResult> AddTestResult(TestResult testResult)
        {
            testResult.Id = Guid.NewGuid();
            testResult.CreatedDate = DateTime.Now.ToLocalTime();
            testResult.IsDeleted = false;
            foreach (var trd in testResult.TestResultDetails)
            {
                trd.TestResultId = testResult.Id;
                trd.IsDeleted = false;
            }
            await work.TestResults.AddAsync(testResult);
            await work.Save();
            return testResult;
        }
    }
}
