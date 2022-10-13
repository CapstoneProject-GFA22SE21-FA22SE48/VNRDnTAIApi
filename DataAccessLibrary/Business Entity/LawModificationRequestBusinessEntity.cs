using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class LawModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public LawModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<LawModificationRequest>> GetLawModificationRequestsAsync()
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(lawModificationRequest => !lawModificationRequest.IsDeleted);
        }

        public async Task<LawModificationRequest>
            GetLawModificationRequestByModifyingParagraphIdAsync(Guid modifyingParagraphId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifyingParagraphId.Equals(modifyingParagraphId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<LawModificationRequest>>
            GetLawModificationRequestsByModifiedParagraphIdAsync(Guid modifiedParagraphId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifiedParagraphId.Equals(modifiedParagraphId));
        }

        public async Task<IEnumerable<LawModificationRequest>>
            GetLawModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.LawModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ScribeId.Equals(scribeId));
        }

        public async Task<LawModificationRequest>
            AddLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            lawModificationRequest.Id = Guid.NewGuid();
            lawModificationRequest.Status = (int)Status.Pending;
            lawModificationRequest.CreatedDate = DateTime.Now;
            lawModificationRequest.IsDeleted = false;
            await work.LawModificationRequests.AddAsync(lawModificationRequest);
            await work.Save();
            return lawModificationRequest;
        }

        public async Task<LawModificationRequest>
            UpdateLawModificationRequest(LawModificationRequest lawModificationRequest)
        {
            work.LawModificationRequests.Update(lawModificationRequest);
            await work.Save();
            return lawModificationRequest;
        }
        public async Task RemoveLawModificationRequestByParagraphId(Guid modifyingParagraphId)
        {
            LawModificationRequest lawModificationRequest =
                (await work.LawModificationRequests.GetAllAsync())
                .Where(lm => !lm.IsDeleted && lm.ModifyingParagraphId == modifyingParagraphId)
                .FirstOrDefault();
            lawModificationRequest.IsDeleted = true;
            work.LawModificationRequests.Update(lawModificationRequest);
            await work.Save();
        }

    }
}
