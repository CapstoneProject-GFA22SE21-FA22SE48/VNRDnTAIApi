using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
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
        public async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            return (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted && question.Status == (int)Status.Active)
                .OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
        }

        //public async Task<IEnumerable<Question> GetAllStudySets(string testCatId) //questioncate
        //{
        //    var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
        //        .Where(question => !question.IsDeleted
        //                && question.Status == (int)Status.Active
        //                && question.TestCategoryId.ToString()
        //                .Equals(testCatId.ToString()))
        //        .OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
        //    return res;
        //}

        public async Task<IEnumerable<Question>> GetStudySetByCategoryAndSeparator(string testCatId, string questionCategoryId, int separator)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.TestCategoryId.ToString().Equals(testCatId.ToString())
                        && question.QuestionCategoryId.ToString().Equals(questionCategoryId.ToString())
                        ).OrderBy(u => int.Parse(u.Name.Split(" ")[1]))
                        //.Skip(25 * separator).Take(25)
                        ;
            return res;
        }

        //***
        public async Task<IEnumerable<Question>> GetRandomTestSetByCategory(string testCatId)
        {
            List<Question> res = new List<Question>();
            double rate = 25 / 7;
            var qs = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted);
            var qcs = (await work.QuestionCategories.GetAllAsync()).Where(qc => !qc.IsDeleted);

            foreach (var qc in qcs)
            {
                if (RandomGen.NextDouble() < (double)(4 / 7))
                {
                    rate = Math.Ceiling(rate);
                }
                else
                {
                    rate = Math.Floor(rate);
                }
                res.AddRange(qc.Questions.OrderBy(x => Guid.NewGuid()).Take((int)rate));
            }

            if (res.Count > 25)
            {
                while (res.Count > 25) res.RemoveAt(res.Count - 1);
            }
            else if (res.Count < 25)
            {
                res.AddRange(qs.Where(q => !q.IsDeleted && !res.Contains(q)).OrderBy(x => Guid.NewGuid()).Take(25 - res.Count));
            }
            foreach (var question in res)
            {
                question.QuestionCategory = null;
            }
            return res;
        }
        public async Task<Question> GetQuestionAsync(Guid id)
        {
            return (await work.Questions.GetAllAsync())
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Question> AddQuestion(Question question)
        {
            question.Id = Guid.NewGuid();
            question.IsDeleted = false;
            await work.Questions.AddAsync(question);
            await work.Save();
            return question;
        }

        public async Task<Question> UpdateQuestion(Question question)
        {
            work.Questions.Update(question);
            await work.Save();
            return question;
        }
        public async Task RemoveQuestion(Guid id)
        {
            Question question = await work.Questions.GetAsync(id);
            question.IsDeleted = true;
            work.Questions.Update(question);
            await work.Save();
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
