using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNRDnTAILibrary.Utilities;

namespace DataAccessLibrary.Business_Entity
{
    public class GpssignBusinessEntity
    {
        private IUnitOfWork work;
        public GpssignBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<Gpssign>> GetGpssignsAsync()
        {
            return (await work.Gpssigns.GetAllAsync())
                .Where(gpssign => !gpssign.IsDeleted);
        }
        public async Task<Gpssign> GetGpssignAsync(Guid id)
        {
            return (await work.Gpssigns.GetAllAsync())
                .Where(gpssign => !gpssign.IsDeleted && gpssign.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Gpssign> AddGpssign(Gpssign gpssign)
        {
            gpssign.Id = Guid.NewGuid();
            if (!gpssign.IsDeleted)
            {
                gpssign.IsDeleted = false;
            }
            await work.Gpssigns.AddAsync(gpssign);
            await work.Save();
            return gpssign;
        }

        public async Task<Gpssign> AddGpsSignDTO(GpsSignDTO gpsSignDTO)
        {
            Sign sign = (await work.Signs.GetAllAsync())
                .Where(s => !s.IsDeleted && s.Status == (int)Status.Active && s.Id == gpsSignDTO.SignId)
                .FirstOrDefault();
            if (sign != null)
            {
                Gpssign gpsSign = new Gpssign();
                gpsSign.Id = Guid.NewGuid();
                gpsSign.SignId = gpsSignDTO.SignId;
                gpsSign.Latitude = gpsSignDTO.Latitude;
                gpsSign.Longtitude = gpsSignDTO.Longitude;
                gpsSign.Status = (int)Status.Deactivated;
                //gpsSign.Status = (int)Status.Deactivated;
                gpsSign.IsDeleted = false;
                await work.Gpssigns.AddAsync(gpsSign);
                await work.Save();
                return gpsSign;
            }
            else
            {
                return new Gpssign();
            }
        }
        public async Task<Gpssign> UpdateGpssign(Gpssign gpssign)
        {
            work.Gpssigns.Update(gpssign);
            await work.Save();
            return gpssign;
        }
        public async Task RemoveGpssign(Guid id)
        {
            Gpssign gpssign = await work.Gpssigns.GetAsync(id);
            gpssign.IsDeleted = true;
            work.Gpssigns.Update(gpssign);
            await work.Save();
        }

        public async Task<IEnumerable<GpsSignDTO>> GetGpssignsNearby(double latitude, double longtitude, double distance)
        {
            var detal = (from gpssigns in
                            (await work.Gpssigns.GetAllAsync())
                            .Where(g => !g.IsDeleted && g.Status == (int)Status.Active)
                         join signs in (await work.Signs.GetAllAsync()).Where(s => !s.IsDeleted && s.Status == (int)Status.Active)
                         on gpssigns.SignId equals signs.Id
                         select new GpsSignDTO
                         {
                             Id = gpssigns.Id,
                             SignId = signs.Id,
                             ImageUrl = signs.ImageUrl,
                             Latitude = gpssigns.Latitude,
                             Longitude = gpssigns.Longtitude
                         }).Where(g =>
                            GpsUtils.GetDistance(latitude, longtitude, (double)g.Latitude, (double)g.Longitude, "KM") <= distance)
                        .ToList();
            return detal;
        }
    }
}
