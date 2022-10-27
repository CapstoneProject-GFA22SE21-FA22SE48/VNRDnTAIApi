using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<SignModificationRequest> GetSignModificationRequestByModifyingSignIdAsync(Guid modifyingSignId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ModifyingSignId.Equals(modifyingSignId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsByModifiedSignIdAsync(Guid modifiedSignId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ModifiedSignId.Equals(modifiedSignId));
        }

        public async Task<IEnumerable<SignModificationRequest>>
            GetSignModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.SignModificationRequests.GetAllAsync())
                .Where(p => p.ScribeId.Equals(scribeId));
        }

        public async Task<SignModificationRequest> AddSignModificationRequest(SignModificationRequest signModificationRequest)
        {
            signModificationRequest.Id = Guid.NewGuid();
            signModificationRequest.Status = (int)Status.Pending;
            signModificationRequest.CreatedDate = DateTime.Now;
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
        public async Task RemoveSignModificationRequest(Guid modifyingSignId)
        {
            SignModificationRequest signModificationRequest = await work.SignModificationRequests.GetAsync(modifyingSignId);
            signModificationRequest.IsDeleted = true;
            work.SignModificationRequests.Update(signModificationRequest);
            await work.Save();
        }

        //--------------------------------------------------
        public async Task<SignModificationRequest> GetSignRomDetail(Guid modifyingSignId)
        {
            SignModificationRequest signRom =
                (await work.SignModificationRequests.GetAllAsync(nameof(SignModificationRequest.ModifyingSign), nameof(SignModificationRequest.ModifiedSign)))
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();
            return signRom;
        }
        //--------------------------------------------------
        public async Task<SignModificationRequest> GetGpssignRomDetail(Guid modifyingGpssignId)
        {
            SignModificationRequest gpssignRom =
                (await work.SignModificationRequests.GetAllAsync(nameof(SignModificationRequest.ModifyingGpssign), nameof(SignModificationRequest.ModifiedGpssign)))
                .Where(s => s.ModifyingGpssignId == modifyingGpssignId).FirstOrDefault();
            return gpssignRom;
        }
    }
}
