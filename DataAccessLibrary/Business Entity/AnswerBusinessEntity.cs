using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class AnswerBusinessEntity
    {
        private IUnitOfWork work;
        public AnswerBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Answer>> GetAnswersAsync()
        {
            return (await work.Answers.GetAllAsync())
                .Where(answer => !answer.IsDeleted);
        }
        public async Task<Answer> GetAnswerAsync(Guid id)
        {
            return (await work.Answers.GetAllAsync())
                .Where(answer => !answer.IsDeleted && answer.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Answer> AddAnswer(Answer answer)
        {
            answer.Id = Guid.NewGuid();
            answer.IsDeleted = false;
            await work.Answers.AddAsync(answer);
            await work.Save();
            return answer;
        }
        public async Task<Answer> UpdateAnswer(Answer answer)
        {
            work.Answers.Update(answer);
            await work.Save();
            return answer;
        }
        public async Task RemoveAnswer(Guid id)
        {
            Answer answer = await work.Answers.GetAsync(id);
            answer.IsDeleted = true;
            work.Answers.Update(answer);
            await work.Save();
        }
    }
}
