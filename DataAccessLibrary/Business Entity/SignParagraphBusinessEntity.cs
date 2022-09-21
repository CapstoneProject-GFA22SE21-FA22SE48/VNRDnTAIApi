using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class SignParagraphBusinessEntity
    {
        private IUnitOfWork work;
        public SignParagraphBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<SignParagraph>> GetSignParagraphsAsync()
        {
            return (await work.SignParagraphs.GetAllAsync())
                .Where(signParagraph => !signParagraph.IsDeleted);
        }
        public async Task<SignParagraph> GetSignParagraphAsync(Guid id)
        {
            return (await work.SignParagraphs.GetAllAsync())
                .Where(signParagraph => !signParagraph.IsDeleted && signParagraph.Id.Equals(id))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SignParagraph>> GetSignParagraphsBySignIdAsync(Guid signId)
        {
            return (await work.SignParagraphs.GetAllAsync())
                .Where(signParagraph => !signParagraph.IsDeleted && signParagraph.SignId.Equals(signId));
        }

        public async Task<SignParagraph> AddSignParagraph(SignParagraph signParagraph)
        {
            signParagraph.Id = Guid.NewGuid();
            signParagraph.IsDeleted = false;
            await work.SignParagraphs.AddAsync(signParagraph);
            await work.Save();
            return signParagraph;
        }
        public async Task<SignParagraph> UpdateSignParagraph(SignParagraph signParagraph)
        {
            work.SignParagraphs.Update(signParagraph);
            await work.Save();
            return signParagraph;
        }
        public async Task RemoveSignParagraph(Guid id)
        {
            SignParagraph signParagraph = await work.SignParagraphs.GetAsync(id);
            signParagraph.IsDeleted = true;
            work.SignParagraphs.Update(signParagraph);
            await work.Save();
        }
    }
}
