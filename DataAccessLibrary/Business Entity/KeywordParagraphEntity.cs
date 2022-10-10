using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class KeywordParagraphBusinessEntity
    {
        private IUnitOfWork work;
        public KeywordParagraphBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<KeywordParagraph>> GetKeywordParagraphsByKeywordIdAsync(Guid keywordId)
        {
            return (await work.KeywordParagraphs.GetAllAsync())
                .Where(kp => !kp.IsDeleted && kp.KeywordId == keywordId);
        }
    }
}
