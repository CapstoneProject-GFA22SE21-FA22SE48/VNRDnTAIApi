using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class DecreeBusinessEntity
    {
        private IUnitOfWork work;
        public DecreeBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Decree>> GetDecreesAsync()
        {
            return (await work.Decrees.GetAllAsync())
                .Where(decree => !decree.IsDeleted);
        }
    }
}
