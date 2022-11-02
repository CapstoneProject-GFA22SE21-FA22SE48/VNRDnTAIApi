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
            if (questionModificationRequest != null)
            {
                questionModificationRequest.IsDeleted = true;
            }

            Question modifyingQuestion = (await work.Questions.GetAllAsync())
                .Where(q => q.Id == modifyingQuestionId)
                .FirstOrDefault();
            if (modifyingQuestion != null)
            {
                modifyingQuestion.IsDeleted = true;
            }

            work.QuestionModificationRequests.Update(questionModificationRequest);
            work.Questions.Update(modifyingQuestion);
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

            //Get data in return for notification adding
            questionModificationRequest.Scribe = (await work.Users.GetAllAsync())
                .Where(u => u.Id == questionModificationRequest.ScribeId)
                .FirstOrDefault();
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
        public async Task<QuestionModificationRequest> ApproveQuestionRom(Guid modifyingQuestionId)
        {
            QuestionModificationRequest questionRom = (await work.QuestionModificationRequests.GetAsync(modifyingQuestionId));

            if (questionRom != null)
            {
                Question modifyingQuestion = await work.Questions.GetAsync(questionRom.ModifyingQuestionId);
                Question modifiedQuestion = null;
                if (questionRom.ModifiedQuestionId != null)
                {
                    modifiedQuestion = await work.Questions.GetAsync((Guid)questionRom.ModifiedQuestionId);
                }

                if (questionRom.OperationType == (int)OperationType.Add)
                {
                    questionRom.Status = (int)Status.Approved;
                    if (modifyingQuestion != null)
                    {
                        modifyingQuestion.Status = (int)Status.Active;
                    }
                }
                else if (questionRom.OperationType == (int)OperationType.Update)
                {
                    questionRom.Status = (int)Status.Approved;
                    if (modifyingQuestion != null)
                    {
                        modifyingQuestion.Status = (int)Status.Active;
                    }
                    if (modifiedQuestion != null)
                    {
                        modifiedQuestion.IsDeleted = true;

                        //Reference all Pending Rom of the modifiedQuestionId to the new modifyingQuestionId
                        IEnumerable<QuestionModificationRequest> questionRomsRefModifiedQuestion =
                            (await work.QuestionModificationRequests.GetAllAsync())
                            .Where(q => q.Status == (int)Status.Pending
                                    && q.ModifiedQuestionId == modifiedQuestion.Id);
                        foreach (QuestionModificationRequest questionMod in questionRomsRefModifiedQuestion)
                        {
                            questionMod.ModifiedQuestionId = modifyingQuestion.Id;
                        }
                    }
                }
                else if (questionRom.OperationType == (int)OperationType.Delete)
                {
                    questionRom.Status = (int)Status.Approved;
                    if (modifyingQuestion != null)
                    {
                        modifyingQuestion.Status = (int)Status.Active;
                    }
                    if (modifiedQuestion != null)
                    {
                        modifiedQuestion.IsDeleted = true;

                        //Set status of all Pending ROM reference to the modifiedQuestion to Confirmed
                        IEnumerable<QuestionModificationRequest> questionRomsRefModifiedQuestion =
                            (await work.QuestionModificationRequests.GetAllAsync())
                            .Where(q => q.Status == (int)Status.Pending
                                    && q.ModifiedQuestionId == modifiedQuestion.Id);

                        foreach (QuestionModificationRequest questionMod in questionRomsRefModifiedQuestion)
                        {
                            questionMod.Status = (int)Status.Confirmed;
                        }
                    }
                }
            }
            await work.Save();

            //include in return to use in notification
            questionRom.Scribe = (await work.Users.GetAsync(questionRom.ScribeId));
            questionRom.Admin = (await work.Users.GetAsync(questionRom.AdminId));
            questionRom.ModifyingQuestion = (await work.Questions.GetAsync(questionRom.ModifyingQuestionId));
            return questionRom;
        }
        //----------------------------------------------------
        public async Task<QuestionModificationRequest> DenyQuestionRom(Guid modifyingQuestionId, string deniedReason)
        {
            QuestionModificationRequest questionRom = (await work.QuestionModificationRequests.GetAsync(modifyingQuestionId));
            if (questionRom != null)
            {
                questionRom.Status = (int)Status.Denied;
                questionRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == questionRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == questionRom.ScribeId && s.Status == (int)Status.Denied).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(q => q.ScribeId == questionRom.ScribeId && q.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == questionRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == questionRom.ScribeId).Count()
                + (await work.QuestionModificationRequests.GetAllAsync())
                .Where(q => q.ScribeId == questionRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User scribe = await work.Users.GetAsync(questionRom.ScribeId);
                    scribe.Status = (int)Status.Deactivated;
                }
            }

            await work.Save();

            //include in return to use in notification
            questionRom.Scribe = (await work.Users.GetAsync(questionRom.ScribeId));
            questionRom.Admin = (await work.Users.GetAsync(questionRom.AdminId));
            questionRom.ModifyingQuestion = (await work.Questions.GetAsync(questionRom.ModifyingQuestionId));
            return questionRom;
        }
        //---------------------------------------------------
        public async Task<QuestionModificationRequest> CancelQuestionRom(Guid modifyingQuestionId)
        {
            QuestionModificationRequest questionRom = (await work.QuestionModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.ModifyingQuestionId == modifyingQuestionId)
                .FirstOrDefault();

            if (questionRom != null)
            {
                questionRom.Status = (int)Status.Cancelled;
            }
            await work.Save();
            return questionRom;
        }

    }
}
