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

        public async Task<ParagraphModificationRequest> GetParagraphModificationRequestByParagraphIdUserIdAsync(Guid paragraphId, Guid userId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ParagraphId.Equals(paragraphId) && p.UserId.Equals(userId))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<ParagraphModificationRequest>> 
            GetParagraphModificationRequestsByParagraphIdAsync(Guid paragraphId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.ParagraphId.Equals(paragraphId));
        }

        public async Task<IEnumerable<ParagraphModificationRequest>> 
            GetParagraphModificationRequestsByUserIdAsync(Guid userId)
        {
            return (await work.ParagraphModificationRequests.GetAllAsync())
                .Where(p => !p.IsDeleted && p.UserId.Equals(userId));
        }

        public async Task<ParagraphModificationRequest> AddParagraphModificationRequest(ParagraphModificationRequest paragraphModificationRequest)
        {
            paragraphModificationRequest.IsDeleted = false;
            await work.ParagraphModificationRequests.AddAsync(paragraphModificationRequest);
            await work.Save();
            return paragraphModificationRequest;
        }
        public async Task<ParagraphModificationRequest> UpdateParagraphModificationRequest(ParagraphModificationRequest paragraphModificationRequest)
        {
            work.ParagraphModificationRequests.Update(paragraphModificationRequest);
            await work.Save();
            return paragraphModificationRequest;
        }
        public async Task RemoveParagraphModificationRequest(Guid paragraphId, Guid userId)
        {
            ParagraphModificationRequest paragraphModificationRequest = await work.ParagraphModificationRequests.GetAsync(paragraphId, userId);
            paragraphModificationRequest.IsDeleted = true;
            work.ParagraphModificationRequests.Update(paragraphModificationRequest);
            await work.Save();
        }
    }
}
