using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            ParagraphModificationRequestAdmins = new HashSet<ParagraphModificationRequest>();
            ParagraphModificationRequestScribes = new HashSet<ParagraphModificationRequest>();
            TestResults = new HashSet<TestResult>();
            UserModificationRequestArbitratingAdmins = new HashSet<UserModificationRequest>();
            UserModificationRequestModifiedUsers = new HashSet<UserModificationRequest>();
            UserModificationRequestModifyingUsers = new HashSet<UserModificationRequest>();
            UserModificationRequestPromotingAdmins = new HashSet<UserModificationRequest>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ParagraphModificationRequest> ParagraphModificationRequestAdmins { get; set; }
        public virtual ICollection<ParagraphModificationRequest> ParagraphModificationRequestScribes { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestArbitratingAdmins { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestModifiedUsers { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestModifyingUsers { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestPromotingAdmins { get; set; }
    }
}
