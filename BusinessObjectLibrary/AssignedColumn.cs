using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class AssignedColumn
    {
        public Guid ColumnId { get; set; }
        public Guid UserId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Column Column { get; set; }
        public virtual User User { get; set; }
    }
}
