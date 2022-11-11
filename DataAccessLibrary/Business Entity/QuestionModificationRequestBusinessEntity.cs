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
            //check if modifiedQuestion is still active
            if (questionModificationRequest.ModifiedQuestionId != null)
            {
                Question modifiedQuestion = await work.Questions.GetAsync((Guid)questionModificationRequest.ModifiedQuestionId);
                if (modifiedQuestion != null)
                {
                    if (modifiedQuestion.Status == (int)Status.Deactivated || modifiedQuestion.IsDeleted == true)
                    {
                        throw new Exception("Câu hỏi không còn khả dụng");
                    }
                }
            }

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
                if (questionRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

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
                if (questionRom.Status == (int)Status.Cancelled)
                {
                    throw new Exception("Yêu cầu đã bị hủy");
                }
            }

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
                    User deactivatingScribe = await work.Users.GetAsync((Guid)questionRom.ScribeId);
                    deactivatingScribe.Status = (int)Status.Deactivated;

                    //Remove all assigned tasks of scribe
                    IEnumerable<AssignedColumn> assignedColumns =
                        (await work.AssignedColumns.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedQuestionCategory> assignedQuestionCategories =
                        (await work.AssignedQuestionCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    IEnumerable<AssignedSignCategory> assignedSignCategories =
                        (await work.AssignedSignCategories.GetAllAsync())
                        .Where(l => !l.IsDeleted && l.ScribeId == deactivatingScribe.Id);

                    foreach (AssignedColumn assignedColumn in assignedColumns)
                    {
                        work.AssignedColumns.Delete(assignedColumn);
                    }

                    foreach (AssignedSignCategory assignedSignCategory in assignedSignCategories)
                    {
                        work.AssignedSignCategories.Delete(assignedSignCategory);
                    }

                    foreach (AssignedQuestionCategory assignedQuestionCategory in assignedQuestionCategories)
                    {
                        work.AssignedQuestionCategories.Delete(assignedQuestionCategory);
                    }

                    //Release all GPSSign roms that are claimed by current scribe
                    IEnumerable<SignModificationRequest> claimedGpssignRoms =
                        (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingGpssignId != null);
                    if (claimedGpssignRoms != null)
                    {
                        foreach (SignModificationRequest gpssignRom in claimedGpssignRoms)
                        {
                            gpssignRom.Status = (int)Status.Pending;
                            gpssignRom.ScribeId = null;
                        }
                    }

                    //Hard delete all Roms of scribe
                    IEnumerable<LawModificationRequest> lawRoms = (await work.LawModificationRequests.GetAllAsync())
                                .Where(rom => rom.ScribeId == deactivatingScribe.Id);
                    IEnumerable<SignModificationRequest> signRoms = (await work.SignModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id && rom.ModifyingSignId != null);
                    IEnumerable<QuestionModificationRequest> questionRoms = (await work.QuestionModificationRequests.GetAllAsync())
                        .Where(rom => rom.ScribeId == deactivatingScribe.Id);

                    if (lawRoms != null)
                    {
                        foreach (LawModificationRequest lawRom in lawRoms)
                        {
                            work.LawModificationRequests.Delete(lawRom);
                        }
                    }

                    if (signRoms != null)
                    {
                        foreach (SignModificationRequest signRom in signRoms)
                        {
                            work.SignModificationRequests.Delete(signRom);
                        }
                    }

                    if (questionRoms != null)
                    {
                        foreach (QuestionModificationRequest qRom in questionRoms)
                        {
                            work.QuestionModificationRequests.Delete(qRom);
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
        //---------------------------------------------------
        public async Task<QuestionModificationRequest> CancelQuestionRom(Guid modifyingQuestionId)
        {
            QuestionModificationRequest questionRom = (await work.QuestionModificationRequests.GetAllAsync())
                .Where(rom => !rom.IsDeleted && rom.ModifyingQuestionId == modifyingQuestionId)
                .FirstOrDefault();

            if (questionRom != null)
            {
                if (questionRom.Status == (int)Status.Approved || questionRom.Status == (int)Status.Denied)
                {
                    throw new Exception("Yêu cầu đã được xử lý");
                }
            }

            if (questionRom != null)
            {
                questionRom.Status = (int)Status.Cancelled;
            }
            await work.Save();
            return questionRom;
        }

    }
}
