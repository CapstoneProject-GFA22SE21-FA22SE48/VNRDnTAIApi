using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Statue
    {
        public Statue()
        {
            Sections = new HashSet<Section>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid VehicleCategoryId { get; set; }
        public Guid ColumnId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Column Column { get; set; }
        public virtual VehicleCategory VehicleCategory { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}
