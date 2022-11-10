using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class VehicleCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public VehicleCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<VehicleCategory>> GetVehicleCategoriesAsync()
        {
            return (await work.VehicleCategories.GetAllAsync())
                .Where(vehicleCategory => !vehicleCategory.IsDeleted)
                .OrderBy(v => v.Name);
        }
    }
}
