using BusinessObjectLibrary;
using System;

namespace DTOsLibrary
{
    public class SignModificationRequestDTO
    {
        public Guid Id { get; set; }
        public Guid? ModifyingSignId { get; set; }
        public Guid? ModifiedSignId { get; set; }
        public Guid? ModifyingGpssignId { get; set; }
        public Guid? ModifiedGpssignId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ScribeId { get; set; }
        public Guid? AdminId { get; set; }
        public int OperationType { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public string DeniedReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Admin { get; set; }
        public virtual Gpssign ModifiedGpssign { get; set; }
        //public virtual Sign ModifiedSign { get; set; }
        public SignDTO ModifiedSign { get; set; }
        public virtual Gpssign ModifyingGpssign { get; set; }
        //public virtual Sign ModifyingSign { get; set; }
        public SignDTO ModifyingSign { get; set; }
        public virtual User Scribe { get; set; }
        public virtual User User { get; set; }
    }
}
