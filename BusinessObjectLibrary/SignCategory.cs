using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class SignCategory
    {
        public SignCategory()
        {
            AssignedSignCategories = new HashSet<AssignedSignCategory>();
            Signs = new HashSet<Sign>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<AssignedSignCategory> AssignedSignCategories { get; set; }
        public virtual ICollection<Sign> Signs { get; set; }
    }
}
