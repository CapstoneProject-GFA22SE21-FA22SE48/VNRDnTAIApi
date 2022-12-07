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
        public async Task<Gpssign> AddGpsSignDTO(GpsSignDTO gpsSignDTO)
        {
            Gpssign gpsSign = new Gpssign();
            gpsSign.Id = Guid.NewGuid();
            if (gpsSignDTO.SignId != Guid.Empty)
            {
                gpsSign.SignId = gpsSignDTO.SignId;
            }
            gpsSign.Latitude = gpsSignDTO.Latitude;
            gpsSign.Longitude = gpsSignDTO.Longitude;
            gpsSign.Status = (int)Status.Deactivated;
            gpsSign.IsDeleted = gpsSignDTO.IsDeleted;
            await work.Gpssigns.AddAsync(gpsSign);
            await work.Save();
            return gpsSign;
        }

        public async Task<IEnumerable<GpsSignDTO>> GetGpssignsNearby(double latitude, double longitude, double distance)
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
                             SignName = signs.Name,
                             ImageUrl = signs.ImageUrl,
                             Latitude = gpssigns.Latitude,
                             Longitude = gpssigns.Longitude
                         })
                         .Where(g =>
                            GpsUtils.GetDistance(latitude, longitude, (double)g.Latitude, (double)g.Longitude, "KM") <= distance)
                        .ToList()
                        ;
            return detal;
        }
    }
}
