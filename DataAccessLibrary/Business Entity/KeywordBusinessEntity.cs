using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System.Collections.Generic;
using System.Linq;
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
            return (await work.Keywords.GetAllAsync()).Where(keyword => !keyword.IsDeleted).GroupBy(k => k.Name).Select(k => k.FirstOrDefault());
        }
    }
}
