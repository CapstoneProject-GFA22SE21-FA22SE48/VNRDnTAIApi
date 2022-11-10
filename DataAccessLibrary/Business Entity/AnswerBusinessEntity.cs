using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId)
        {
            return (await work.Answers.GetAllAsync())
                .Where(answer => !answer.IsDeleted && answer.QuestionId == questionId);
        }
    }
}
