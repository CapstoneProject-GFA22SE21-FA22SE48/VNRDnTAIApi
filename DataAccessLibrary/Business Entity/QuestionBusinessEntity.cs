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

        public async Task<IEnumerable<Question>> GetStudySetByCategoryAndSeparator(string testCatId, int separator)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted && question.TestCategoryId.ToString().Equals(testCatId.ToString())).OrderBy(u => int.Parse(u.Name.Split(" ")[1])).Skip(25 * separator).Take(25);
            return res;
        }

        public async Task<IEnumerable<Question>> GetRandomTestSetByCategory(string testCatId)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted && question.TestCategoryId.ToString().Equals(testCatId.ToString())).OrderBy(r => Guid.NewGuid()).Take(25);
            return res;
        }
        public async Task<Question> GetQuestionAsync(Guid id)
        {
            return (await work.Questions.GetAllAsync())
                .Where(question => !question.IsDeleted && question.Id.Equals(id))
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
    }
}
