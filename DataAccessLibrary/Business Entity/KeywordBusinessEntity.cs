using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class KeywordBusinessEntity
    {
        private IUnitOfWork work;
        public KeywordBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Keyword>> GetKeywordsAsync()
        {
            return (await work.Keywords.GetAllAsync())
                .Where(keyword => !keyword.IsDeleted);
        }
        public async Task<Keyword> GetKeywordAsync(Guid id)
        {
            return (await work.Keywords.GetAllAsync())
                .Where(keyword => !keyword.IsDeleted && keyword.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Keyword> AddKeyword(Keyword keyword)
        {
            keyword.Id = Guid.NewGuid();
            keyword.IsDeleted = false;
            await work.Keywords.AddAsync(keyword);
            await work.Save();
            return keyword;
        }
        public async Task<Keyword> UpdateKeyword(Keyword keyword)
        {
            work.Keywords.Update(keyword);
            await work.Save();
            return keyword;
        }
        public async Task RemoveKeyword(Guid id)
        {
            Keyword keyword = await work.Keywords.GetAsync(id);
            keyword.IsDeleted = true;
            work.Keywords.Update(keyword);
            await work.Save();
        }
    }
}
