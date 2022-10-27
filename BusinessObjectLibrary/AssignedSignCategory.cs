using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class AssignedSignCategory
    {
        public Guid SignCategoryId { get; set; }
        public Guid ScribeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Scribe { get; set; }
        public virtual SignCategory SignCategory { get; set; }
    }
}
