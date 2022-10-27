using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
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
    }
}
