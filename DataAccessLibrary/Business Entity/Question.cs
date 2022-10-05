using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .Where(question => !question.IsDeleted);
        }

        public async Task<IEnumerable<Question>> GetRandomTestSetByCategory(string categoryName)
        {
            var testCatId = (await work.TestCategories.GetAllAsync()).First(cat => cat.Name == categoryName).Id;
            return (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted && question.TestCategoryId.ToString().Equals(testCatId.ToString())).Take(20);
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
