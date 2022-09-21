using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class SignModificationRequest
    {
        public Guid SignId { get; set; }
        public Guid UserId { get; set; }
        public int OperationType { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Sign Sign { get; set; }
        public virtual User User { get; set; }
    }
}
