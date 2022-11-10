using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class QuestionBusinessEntity
    {
        private IUnitOfWork work;
        public static Random RandomGen = new Random();
        public QuestionBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<QuestionDTO>> GetAssigedQuestionsAsync(Guid scribeId)
        {
            //return (await work.Questions.GetAllAsync(nameof(Question.Answers)))
            //    .Where(question => !question.IsDeleted && question.Status == (int)Status.Active)
            //    .OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
            IEnumerable<QuestionDTO> questionDTOs =
                from assignedQuestionCategory in (await work.AssignedQuestionCategories.GetAllAsync())
                                                    .Where(aqc => !aqc.IsDeleted && aqc.ScribeId == scribeId)
                join questionCategory in (await work.QuestionCategories.GetAllAsync())
                .Where(qc => !qc.IsDeleted)
                on assignedQuestionCategory.QuestionCategoryId equals questionCategory.Id
                join question in (await work.Questions.GetAllAsync())
                .Where(question => !question.IsDeleted && question.Status == (int)Status.Active)
                on questionCategory.Id equals question.QuestionCategoryId
                join testCategory in (await work.TestCategories.GetAllAsync())
                .Where(tc => !tc.IsDeleted)
                on question.TestCategoryId equals testCategory.Id

                select new QuestionDTO
                {
                    Id = question.Id,
                    Content = question.Content,
                    ImageUrl = question.ImageUrl,
                    TestCategoryId = question.TestCategoryId,
                    TestCategoryName = testCategory.Name,
                    QuestionCategoryId = question.QuestionCategoryId,
                    QuestionCategoryName = questionCategory.Name
                };
            return questionDTOs;
        }

        public async Task<IEnumerable<Question>> GetStudySetByCategoryAndSeparator(string testCatId, string questionCategoryId, int separator)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.TestCategoryId.ToString().Equals(testCatId.ToString())
                        && question.QuestionCategoryId.ToString().Equals(questionCategoryId.ToString())
                        )
                        //.Skip(25 * separator).Take(25)
                        ;
            return res;
        }

        public async Task<IEnumerable<Question>> GetRandomTestSetByCategory(string testCatId)
        {
            List<Question> res = new List<Question>();

            var noOfQuestionCat = 7;

            //Get noOfQuestionCat by testCat
            noOfQuestionCat = (await work.Questions.GetAllAsync()).Where(q => q.TestCategoryId.ToString() == testCatId)
                .GroupBy(g => new { g.QuestionCategoryId })
                         .Select(g => g.First())
                         .ToList()
                         .Count();

            double rate = 25 / noOfQuestionCat;
            var qs = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted && question.Status == (int)Status.Active && question.TestCategoryId.ToString() == testCatId);
            var qcs = (await work.QuestionCategories.GetAllAsync()).Where(qc => !qc.IsDeleted);

            foreach (var qc in qcs)
            {
                if (RandomGen.NextDouble() < (double)(4 / noOfQuestionCat))
                {
                    rate = Math.Ceiling(rate);
                }
                else
                {
                    rate = Math.Floor(rate);
                }
                res.AddRange(qc.Questions.Where(question => !question.IsDeleted && question.Status == (int)Status.Active && question.TestCategoryId.ToString() == testCatId).OrderBy(x => Guid.NewGuid()).Take((int)rate));
            }

            res = res.DistinctBy(x => x.Content).OrderBy(x => Guid.NewGuid()).ToList();

            if (res.Count > 25)
            {
                while (res.Count > 25) res.RemoveAt(res.Count - 1);
            }
            else if (res.Count < 25)
            {
                res.AddRange(qs.Where(q => !q.IsDeleted && !res.Any(r => r.Content == q.Content)).OrderBy(x => Guid.NewGuid()).Take(25 - res.Count));
            }
            foreach (var question in res)
            {
                question.QuestionCategory = null;
            }
            return res;
        }

        //----------------------------------------------------
        public async Task<Question> AddQuestionForROM(Question question)
        {
            //Add new deactivated question
            question.TestCategory = null;
            question.Id = Guid.NewGuid();


            //If the question is for Delete ROM, then keep IsDeleted = true
            if (question.IsDeleted == true) { }
            else
            {
                question.IsDeleted = false;
            }
            question.Status = (int)Status.Deactivated;

            //Add new question answers
            foreach (Answer answer in question.Answers)
            {
                answer.Id = Guid.NewGuid();
                answer.QuestionId = question.Id;
                //If the answer is for Delete ROM, then keep IsDeleted = true
                if (answer.IsDeleted == true) { }
                else
                {
                    answer.IsDeleted = false;
                }
            }

            await work.Questions.AddAsync(question);
            await work.Save();
            return question;
        }
    }
}
