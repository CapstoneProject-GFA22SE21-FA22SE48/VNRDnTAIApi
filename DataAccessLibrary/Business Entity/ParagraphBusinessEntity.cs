using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class ParagraphBusinessEntity
    {
        private IUnitOfWork work;
        public ParagraphBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Paragraph>> GetParagraphsAsync()
        {
            return (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted);
        }
        public async Task<Paragraph> GetParagraphAsync(Guid id)
        {
            return (await work.Paragraphs.GetAllAsync())
                .Where(paragraph => !paragraph.IsDeleted && paragraph.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Paragraph> AddParagraph(Paragraph paragraph)
        {
            paragraph.Id = Guid.NewGuid();
            paragraph.IsDeleted = false;
            await work.Paragraphs.AddAsync(paragraph);
            await work.Save();
            return paragraph;
        }
        public async Task<Paragraph> UpdateParagraph(Paragraph paragraph)
        {
            work.Paragraphs.Update(paragraph);
            await work.Save();
            return paragraph;
        }
        public async Task RemoveParagraph(Guid id)
        {
            Paragraph paragraph = await work.Paragraphs.GetAsync(id);
            paragraph.IsDeleted = true;
            work.Paragraphs.Update(paragraph);
            await work.Save();
        }
    }
}
