using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class TestResultDetail
    {
        public Guid Id { get; set; }
        public Guid TestResultId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
        public virtual TestResult TestResult { get; set; }
    }
}
