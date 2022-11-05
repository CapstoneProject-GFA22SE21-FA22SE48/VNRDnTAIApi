using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
    }
}
