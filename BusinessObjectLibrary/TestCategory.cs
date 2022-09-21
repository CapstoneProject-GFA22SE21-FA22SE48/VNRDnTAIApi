using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class TestCategory
    {
        public TestCategory()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
