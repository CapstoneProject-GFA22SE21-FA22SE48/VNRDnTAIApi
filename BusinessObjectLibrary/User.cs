using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class User
    {
        public User()
        {
            ParagraphModificationRequests = new HashSet<ParagraphModificationRequest>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ParagraphModificationRequest> ParagraphModificationRequests { get; set; }
    }
}
