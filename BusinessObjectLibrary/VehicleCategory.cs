using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class VehicleCategory
    {
        public VehicleCategory()
        {
            Sections = new HashSet<Section>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Section> Sections { get; set; }
    }
}
