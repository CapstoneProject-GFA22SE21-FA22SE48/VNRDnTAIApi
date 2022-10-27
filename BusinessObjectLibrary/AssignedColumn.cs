using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class AssignedColumn
    {
        public Guid ColumnId { get; set; }
        public Guid ScribeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Column Column { get; set; }
        public virtual User Scribe { get; set; }
    }
}
