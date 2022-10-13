using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class User
    {
        public User()
        {
            AssignedColumns = new HashSet<AssignedColumn>();
            Comments = new HashSet<Comment>();
            LawModificationRequestAdmins = new HashSet<LawModificationRequest>();
            LawModificationRequestScribes = new HashSet<LawModificationRequest>();
            QuestionModificationRequestAdmins = new HashSet<QuestionModificationRequest>();
            QuestionModificationRequestScribes = new HashSet<QuestionModificationRequest>();
            SignModificationRequestAdmins = new HashSet<SignModificationRequest>();
            SignModificationRequestScribes = new HashSet<SignModificationRequest>();
            SignModificationRequestUsers = new HashSet<SignModificationRequest>();
            TestResults = new HashSet<TestResult>();
            UserModificationRequestArbitratingAdmins = new HashSet<UserModificationRequest>();
            UserModificationRequestModifiedUsers = new HashSet<UserModificationRequest>();
            UserModificationRequestPromotingAdmins = new HashSet<UserModificationRequest>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string Gmail { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserModificationRequest UserModificationRequestModifyingUser { get; set; }
        public virtual ICollection<AssignedColumn> AssignedColumns { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestAdmins { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestScribes { get; set; }
        public virtual ICollection<QuestionModificationRequest> QuestionModificationRequestAdmins { get; set; }
        public virtual ICollection<QuestionModificationRequest> QuestionModificationRequestScribes { get; set; }
        public virtual ICollection<SignModificationRequest> SignModificationRequestAdmins { get; set; }
        public virtual ICollection<SignModificationRequest> SignModificationRequestScribes { get; set; }
        public virtual ICollection<SignModificationRequest> SignModificationRequestUsers { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestArbitratingAdmins { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestModifiedUsers { get; set; }
        public virtual ICollection<UserModificationRequest> UserModificationRequestPromotingAdmins { get; set; }
    }
}
