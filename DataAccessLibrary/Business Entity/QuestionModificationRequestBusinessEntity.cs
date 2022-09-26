using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class QuestionModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public QuestionModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<QuestionModificationRequest>> GetQuestionModificationRequestsAsync()
        {
            return (await work.QuestionModificationRequests.GetAllAsync())
                .Where(questionModificationRequest => !questionModificationRequest.IsDeleted);
        }

        public async Task<QuestionModificationRequest>
            GetQuestionModificationRequestByModifyingQuestionIdAsync(Guid modifyingQuestionId)
        {
            return (await work.QuestionModificationRequests.GetAllAsync())
                .Where(q => !q.IsDeleted && q.ModifyingQuestionId.Equals(modifyingQuestionId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<QuestionModificationRequest>>
            GetQuestionModificationRequestsByModifiedQuestionIdAsync(Guid modifiedQuestionId)
        {
            return (await work.QuestionModificationRequests.GetAllAsync())
                .Where(q => !q.IsDeleted && q.ModifiedQuestionId.Equals(modifiedQuestionId));
        }

        public async Task<IEnumerable<QuestionModificationRequest>>
            GetQuestionModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.QuestionModificationRequests.GetAllAsync())
                .Where(q => !q.IsDeleted && q.ScribeId.Equals(scribeId));
        }

        public async Task<QuestionModificationRequest>
            AddQuestionModificationRequest(QuestionModificationRequest questionModificationRequest)
        {
            questionModificationRequest.IsDeleted = false;
            await work.QuestionModificationRequests.AddAsync(questionModificationRequest);
            await work.Save();
            return questionModificationRequest;
        }
        public async Task<QuestionModificationRequest>
            UpdateQuestionModificationRequest(QuestionModificationRequest questionModificationRequest)
        {
            work.QuestionModificationRequests.Update(questionModificationRequest);
            await work.Save();
            return questionModificationRequest;
        }
        public async Task RemoveQuestionModificationRequest(Guid modifyingQuestionId)
        {
            QuestionModificationRequest questionModificationRequest =
                await work.QuestionModificationRequests.GetAsync(modifyingQuestionId);
            questionModificationRequest.IsDeleted = true;
            work.QuestionModificationRequests.Update(questionModificationRequest);
            await work.Save();
        }
    }
}
