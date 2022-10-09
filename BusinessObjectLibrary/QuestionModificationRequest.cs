using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class QuestionModificationRequest
    {
        public Guid ModifyingQuestionId { get; set; }
        public Guid? ModifiedQuestionId { get; set; }
        public Guid ScribeId { get; set; }
        public Guid AdminId { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Admin { get; set; }
        public virtual Question ModifiedQuestion { get; set; }
        public virtual Question ModifyingQuestion { get; set; }
        public virtual User Scribe { get; set; }
    }
}
