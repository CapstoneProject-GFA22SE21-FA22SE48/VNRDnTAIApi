using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class Answer
    {
        public Answer()
        {
            TestResultDetails = new HashSet<TestResultDetail>();
        }

        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }
    }
}
