using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //-----------------------------------------------------------
        // This ROM can be used in add, update, delete a Question
        public async Task<QuestionModificationRequest>
            CreateQuestionModificationRequest(QuestionModificationRequest questionModificationRequest)
        {
            questionModificationRequest.Status = (int)Status.Pending;
            questionModificationRequest.CreatedDate = DateTime.Now;
            questionModificationRequest.IsDeleted = false;
            await work.QuestionModificationRequests.AddAsync(questionModificationRequest);
            await work.Save();
            return questionModificationRequest;
        }
        //--------------------------------------------------
        public async Task<QuestionModificationRequest> GetQuestionRomDetail(Guid modifyingQuestionId)
        {
            QuestionModificationRequest questionRom =
                (await work.QuestionModificationRequests.GetAllMultiIncludeAsync(
                    include: questionRom => questionRom
                    .Include(q => q.ModifyingQuestion)
                    .ThenInclude(q => q.TestCategory)

                    .Include(q => q.ModifyingQuestion)
                    .ThenInclude(q => q.QuestionCategory)

                    .Include(q => q.ModifyingQuestion)
                    .ThenInclude(q => q.Answers)

                    .Include(q => q.ModifiedQuestion)
                    .ThenInclude(q => q.TestCategory)

                    .Include(q => q.ModifiedQuestion)
                    .ThenInclude(q => q.QuestionCategory)

                    .Include(q => q.ModifiedQuestion)
                    .ThenInclude(q => q.Answers)
                    ))
                .Where(q => q.ModifyingQuestionId == modifyingQuestionId).FirstOrDefault();
            return questionRom;
        }
        //--------------------------------------------------
    }
}
