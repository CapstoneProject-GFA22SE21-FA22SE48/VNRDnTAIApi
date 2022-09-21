using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Decree
    {
        public Decree()
        {
            Columns = new HashSet<Column>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Column> Columns { get; set; }
    }
}
