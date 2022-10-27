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
                (await work.SignModificationRequests.GetAllMultiIncludeAsync(
                    include: signRom => signRom
                    .Include(s => s.ModifyingSign)
                    .ThenInclude(s => s.SignCategory)
                    .Include(s => s.ModifiedSign)
                    .ThenInclude(s => s.SignCategory)
                    ))
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
        //--------------------------------------------------
        public async Task<SignModificationRequest> ApproveSignRom(Guid modifyingSignId)
        {
            SignModificationRequest signRom = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();

            if (signRom != null)
            {
                Sign modifyingSign = await work.Signs.GetAsync((Guid)signRom.ModifyingSignId);
                Sign modifiedSign = null;
                if (signRom.ModifiedSignId != null)
                {
                    modifiedSign = await work.Signs.GetAsync((Guid)signRom.ModifiedSignId);
                }

                if (signRom.OperationType == (int)OperationType.Add)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                }
                else if (signRom.OperationType == (int)OperationType.Update)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                    if (modifiedSign != null)
                    {
                        modifiedSign.IsDeleted = true;

                        //Reference all Pending Rom of the modifiedSignId to the new modifyingSignId
                        IEnumerable<SignModificationRequest> signRomsRefModifiedSign =
                            (await work.SignModificationRequests.GetAllAsync())
                            .Where(s => s.Status == (int)Status.Pending
                                    && s.ModifiedSignId == modifiedSign.Id);
                        foreach (SignModificationRequest signMod in signRomsRefModifiedSign)
                        {
                            signMod.ModifiedSignId = modifyingSign.Id;
                        }
                    }
                }
                else if (signRom.OperationType == (int)OperationType.Delete)
                {
                    signRom.Status = (int)Status.Approved;
                    if (modifyingSign != null)
                    {
                        modifyingSign.Status = (int)Status.Active;
                    }
                    if (modifiedSign != null)
                    {
                        modifiedSign.IsDeleted = true;

                        //Set status of all Pending ROM reference to the modifiedSignId to Confirmed
                        IEnumerable<SignModificationRequest> signRomsRefModifiedSign =
                            (await work.SignModificationRequests.GetAllAsync())
                            .Where(s => s.Status == (int)Status.Pending
                                    && s.ModifiedSignId == modifiedSign.Id);

                        foreach (SignModificationRequest signMod in signRomsRefModifiedSign)
                        {
                            signMod.Status = (int)Status.Confirmed;
                        }
                    }
                }
            }
            await work.Save();
            return signRom;
        }
        //----------------------------------------------------
        public async Task<SignModificationRequest> DenySignRom(Guid modifyingSignId, string deniedReason)
        {
            SignModificationRequest signRom = (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ModifyingSignId == modifyingSignId).FirstOrDefault();
            if (signRom != null)
            {
                signRom.Status = (int)Status.Denied;
                signRom.DeniedReason = deniedReason;

                //Calculate approval rate
                double approvalRate = 1 - ((double)((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == signRom.ScribeId && l.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == signRom.ScribeId && s.Status == (int)Status.Denied).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == signRom.ScribeId && s.Status == (int)Status.Denied).Count())
                    /
                ((await work.LawModificationRequests.GetAllAsync())
                    .Where(l => l.ScribeId == signRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                    .Where(s => s.ScribeId == signRom.ScribeId).Count()
                + (await work.SignModificationRequests.GetAllAsync())
                .Where(s => s.ScribeId == signRom.ScribeId).Count()));
                if (approvalRate < 0.65)
                {
                    User scribe = await work.Users.GetAsync((Guid)signRom.ScribeId);
                    scribe.Status = (int)Status.Deactivated;
                }
            }

            await work.Save();
            return signRom;
        }
    }
}
