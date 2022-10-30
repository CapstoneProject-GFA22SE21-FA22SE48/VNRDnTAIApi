using System;

namespace DTOsLibrary
{
    public class GpsSignDTO
    {
        public Guid Id { get; set; }
        public Guid SignId { get; set; }
        public String imageUrl { get; set; }
        public decimal latitude { get; set; }
        public decimal longtitude { get; set; }

    }
}
