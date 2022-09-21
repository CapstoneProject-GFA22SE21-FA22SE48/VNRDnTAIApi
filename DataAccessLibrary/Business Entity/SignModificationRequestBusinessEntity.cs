using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public SignModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<SignModificationRequest>> GetSignModificationRequestsAsync()
        {
            return await work.SignModificationRequests.GetAllAsync();
        }

        public async Task<SignModificationRequest> GetSignModificationRequestBySignIdUserIdAsync(Guid signId, Guid userId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.SignId.Equals(signId) && p.UserId.Equals(userId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsBySignIdAsync(Guid signId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.SignId.Equals(signId));
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsByUserIdAsync(Guid userId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.UserId.Equals(userId));
        }

        public async Task<SignModificationRequest> AddSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            signModificationRequest.IsDeleted = false;
            await work.SignModificationRequests.AddAsync(signModificationRequest);
            await work.Save();
            return signModificationRequest;
        }
        public async Task<SignModificationRequest> UpdateSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            work.SignModificationRequests.Update(signModificationRequest);
            await work.Save();
            return signModificationRequest;
        }
        public async Task RemoveSignModificationRequest(Guid signId, Guid userId)
        {
            SignModificationRequest signModificationRequest = await work.SignModificationRequests.GetAsync(signId, userId);
            signModificationRequest.IsDeleted = true;
            work.SignModificationRequests.Update(signModificationRequest);
            await work.Save();
        }
    }
}
