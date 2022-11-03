using System;

namespace DTOsLibrary
{
    public class GpsSignDTO
    {
        public Guid Id { get; set; }
        public Guid SignId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }

    }
}
