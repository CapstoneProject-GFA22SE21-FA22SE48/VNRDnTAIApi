using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class Gpssign
    {
        public Gpssign()
        {
            SignModificationRequestModifiedGpssigns = new HashSet<SignModificationRequest>();
            SignModificationRequestModifyingGpssigns = new HashSet<SignModificationRequest>();
        }

        public Guid Id { get; set; }
        public Guid? SignId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Sign Sign { get; set; }
        public virtual ICollection<SignModificationRequest> SignModificationRequestModifiedGpssigns { get; set; }
        public virtual ICollection<SignModificationRequest> SignModificationRequestModifyingGpssigns { get; set; }
    }
}
