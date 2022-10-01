using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class SignModificationRequest
    {
        public Guid ModifiedSignId { get; set; }
        public Guid ModifyingSignId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ScribeId { get; set; }
        public Guid? AdminId { get; set; }
        public int OperationType { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Admin { get; set; }
        public virtual Sign ModifiedSign { get; set; }
        public virtual Sign ModifyingSign { get; set; }
        public virtual User Scribe { get; set; }
        public virtual User User { get; set; }
    }
}
