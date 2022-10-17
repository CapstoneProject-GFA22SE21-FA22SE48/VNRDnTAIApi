using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
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
        public async Task<VehicleCategory> GetVehicleCategoryAsync(Guid id)
        {
            return (await work.VehicleCategories.GetAllAsync())
                .Where(vehicleCategory => !vehicleCategory.IsDeleted && vehicleCategory.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<VehicleCategory> AddVehicleCategory(VehicleCategory vehicleCategory)
        {
            vehicleCategory.Id = Guid.NewGuid();
            vehicleCategory.IsDeleted = false;
            await work.VehicleCategories.AddAsync(vehicleCategory);
            await work.Save();
            return vehicleCategory;
        }
        public async Task<VehicleCategory> UpdateVehicleCategory(VehicleCategory vehicleCategory)
        {
            work.VehicleCategories.Update(vehicleCategory);
            await work.Save();
            return vehicleCategory;
        }
        public async Task RemoveVehicleCategory(Guid id)
        {
            VehicleCategory vehicleCategory = await work.VehicleCategories.GetAsync(id);
            vehicleCategory.IsDeleted = true;
            work.VehicleCategories.Update(vehicleCategory);
            await work.Save();
        }
    }
}
