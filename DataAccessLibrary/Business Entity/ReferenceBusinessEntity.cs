using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class ReferenceBusinessEntity
    {
        private IUnitOfWork work;
        public ReferenceBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Reference>> GetReferencesAsync()
        {
            return await work.References.GetAllAsync();
        }

        public async Task<IEnumerable<Reference>> GetReferencesByParagraphIdAsync(Guid paragraphId)
        {
            return (await work.References.GetAllAsync())
                .Where(r => r.ParagraphId.Equals(paragraphId));
        }

        public async Task<Reference> AddReference(Reference reference)
        {
            await work.References.AddAsync(reference);
            await work.Save();
            return reference;
        }
        public async Task<Reference> UpdateReference(Reference reference)
        {
            work.References.Update(reference);
            await work.Save();
            return reference;
        }
        public async Task RemoveReference(Guid paragraphId, Guid referenceParagraphId)
        {
            Reference reference = await work.References.GetAsync(paragraphId, referenceParagraphId);
            work.References.Delete(reference);
            await work.Save();
        }
    }
}
