using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class ParagraphModificationRequestBusinessEntity
    {
        private IUnitOfWork work;
        public ParagraphModificationRequestBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<ParagraphModificationRequest>> GetParagraphModificationRequestsAsync()
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(paragraphModificationRequest => !paragraphModificationRequest.IsDeleted);
        }

        public async Task<ParagraphModificationRequest> 
            GetParagraphModificationRequestByModifyingParagraphIdAsync(Guid modifyingParagraphId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifyingParagraphId.Equals(modifyingParagraphId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<ParagraphModificationRequest>> 
            GetParagraphModificationRequestsByModifiedParagraphIdAsync(Guid modifiedParagraphId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ModifiedParagraphId.Equals(modifiedParagraphId));
        }

        public async Task<IEnumerable<ParagraphModificationRequest>> 
            GetParagraphModificationRequestsByScribeIdAsync(Guid scribeId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ScribeId.Equals(scribeId));
        }

        public async Task<ParagraphModificationRequest> 
            AddParagraphModificationRequest(ParagraphModificationRequest paragraphModificationRequest)
        {
            paragraphModificationRequest.IsDeleted = false;
            await work.ParagraphModificationRequests.AddAsync(paragraphModificationRequest);
            await work.Save();
            return paragraphModificationRequest;
        }
        public async Task<ParagraphModificationRequest> 
            UpdateParagraphModificationRequest(ParagraphModificationRequest paragraphModificationRequest)
        {
            work.ParagraphModificationRequests.Update(paragraphModificationRequest);
            await work.Save();
            return paragraphModificationRequest;
        }
        public async Task RemoveParagraphModificationRequest(Guid modifyingParagraphId)
        {
            ParagraphModificationRequest paragraphModificationRequest = 
                await work.ParagraphModificationRequests.GetAsync(modifyingParagraphId);
            paragraphModificationRequest.IsDeleted = true;
            work.ParagraphModificationRequests.Update(paragraphModificationRequest);
            await work.Save();
        }
    }
}
