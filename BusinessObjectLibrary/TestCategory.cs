using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class TestCategory
    {
        public TestCategory()
        {
            QuestionCategories = new HashSet<QuestionCategory>();
            Questions = new HashSet<Question>();
            TestResults = new HashSet<TestResult>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<QuestionCategory> QuestionCategories { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
