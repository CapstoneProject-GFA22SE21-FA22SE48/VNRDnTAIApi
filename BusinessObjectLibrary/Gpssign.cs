using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Gpssign
    {
        public Guid Id { get; set; }
        public Guid SignId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Sign Sign { get; set; }
    }
}
