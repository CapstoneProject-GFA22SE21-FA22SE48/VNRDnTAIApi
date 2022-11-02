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
    public class UserModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public UserModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<UserModificationRequest>> GetUserModificationRequestsAsync()
        {
            return (await work.UserModificationRequests.GetAllAsync())
                .Where(userModificationRequest => !userModificationRequest.IsDeleted);
        }

        public async Task<UserModificationRequest>
            GetUserModificationRequestByModifyingUserIdAsync(Guid modifyingUserId)
        {
            return (await work.UserModificationRequests.GetAllAsync())
                .Where(u => !u.IsDeleted && u.ModifyingUserId.Equals(modifyingUserId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<UserModificationRequest>>
            GetUserModificationRequestsByModifiedUserIdAsync(Guid modifiedUserId)
        {
            return (await work.UserModificationRequests.GetAllAsync())
                .Where(u => !u.IsDeleted && u.ModifiedUserId.Equals(modifiedUserId));
        }

        public async Task<UserModificationRequest>
            AddUserModificationRequest(UserModificationRequest userModificationRequest)
        {
            userModificationRequest.IsDeleted = false;
            await work.UserModificationRequests.AddAsync(userModificationRequest);
            await work.Save();
            return userModificationRequest;
        }
        public async Task<UserModificationRequest>
            UpdateUserModificationRequest(UserModificationRequest userModificationRequest)
        {
            work.UserModificationRequests.Update(userModificationRequest);
            await work.Save();
            return userModificationRequest;
        }
        public async Task RemoveUserModificationRequest(Guid modifyingUserId)
        {
            UserModificationRequest userModificationRequest =
                await work.UserModificationRequests.GetAsync(modifyingUserId);
            userModificationRequest.IsDeleted = true;
            work.UserModificationRequests.Update(userModificationRequest);
            await work.Save();
        }
        //--------------------------------------------------
        public async Task<UserModificationRequest> GetUserRomDetail(Guid modifyingUserId)
        {
            UserModificationRequest userRom =
                (await work.UserModificationRequests.GetAllMultiIncludeAsync(
                    include: userRom => userRom
                    .Include(u => u.ModifyingUser)
                    .Include(u => u.ModifiedUser)
                    .Include(u => u.PromotingAdmin)
                    .Include(u => u.ArbitratingAdmin)
                    ))
                .Where(u => u.ModifyingUserId == modifyingUserId).FirstOrDefault();
            return userRom;
        }
        //--------------------------------------------------
        public async Task<UserModificationRequest> ApproveUserRom(Guid modifyingUserId)
        {
            UserModificationRequest userRom = (await work.UserModificationRequests.GetAllAsync())
                .Where(u => u.ModifyingUserId == modifyingUserId)
                .FirstOrDefault();

            if (userRom != null)
            {
                User modifyingUser = await work.Users.GetAsync(userRom.ModifyingUserId);
                User modifiedUser = await work.Users.GetAsync((Guid)userRom.ModifiedUserId);

                userRom.Status = (int)Status.Approved;
                modifyingUser.Status = (int)Status.Active;
                modifiedUser.IsDeleted = true;

                //Set status of all pending Rom of modifiedUserId to confirmed
                IEnumerable<LawModificationRequest> lawRoms =
                    (await work.LawModificationRequests.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id && l.Status == (int)Status.Pending);
                IEnumerable<SignModificationRequest> signRoms =
                    (await work.SignModificationRequests.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id && l.Status == (int)Status.Pending);
                IEnumerable<QuestionModificationRequest> questionRoms =
                    (await work.QuestionModificationRequests.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id && l.Status == (int)Status.Pending);

                foreach (LawModificationRequest lawRom in lawRoms)
                {
                    lawRom.Status = (int)Status.Confirmed;
                }

                foreach (SignModificationRequest signRom in signRoms)
                {
                    signRom.Status = (int)Status.Confirmed;
                }

                foreach (QuestionModificationRequest questionRom in questionRoms)
                {
                    questionRom.Status = (int)Status.Confirmed;
                }

                //Remove all assigned tasks of scribe
                IEnumerable<AssignedColumn> assignedColumns =
                    (await work.AssignedColumns.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id);

                IEnumerable<AssignedQuestionCategory> assignedQuestionCategories =
                    (await work.AssignedQuestionCategories.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id);

                IEnumerable<AssignedSignCategory> assignedSignCategories =
                    (await work.AssignedSignCategories.GetAllAsync())
                    .Where(l => !l.IsDeleted && l.ScribeId == modifiedUser.Id);

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

            }
            await work.Save();

            //include in return to use in notification
            userRom.PromotingAdmin = (await work.Users.GetAsync(userRom.PromotingAdminId));
            userRom.ArbitratingAdmin = (await work.Users.GetAsync(userRom.ArbitratingAdminId));
            userRom.ModifyingUser = (await work.Users.GetAsync(userRom.ModifyingUserId));
            return userRom;
        }
        //----------------------------------------------------
        public async Task<UserModificationRequest> DenyUserRom(Guid modifyingUserId, string deniedReason)
        {
            UserModificationRequest userRom = (await work.UserModificationRequests.GetAllAsync())
                .Where(u => u.ModifyingUserId == modifyingUserId)
                .FirstOrDefault();
            if (userRom != null)
            {
                userRom.Status = (int)Status.Denied;
                userRom.DeniedReason = deniedReason;
            }

            await work.Save();

            //include in return to use in notification
            userRom.PromotingAdmin = (await work.Users.GetAsync(userRom.PromotingAdminId));
            userRom.ModifyingUser = (await work.Users.GetAsync(userRom.ModifyingUserId));
            userRom.ArbitratingAdmin = (await work.Users.GetAsync(userRom.ArbitratingAdminId));
            return userRom;
        }
    }
}
