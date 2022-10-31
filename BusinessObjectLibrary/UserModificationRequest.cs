using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class UserModificationRequest
    {
        public Guid ModifyingUserId { get; set; }
        public Guid ModifiedUserId { get; set; }
        public Guid PromotingAdminId { get; set; }
        public Guid ArbitratingAdminId { get; set; }
        public int Status { get; set; }
        public string DeniedReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User ArbitratingAdmin { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual User ModifyingUser { get; set; }
        public virtual User PromotingAdmin { get; set; }
    }
}
