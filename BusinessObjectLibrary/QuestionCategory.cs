using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class QuestionCategory
    {
        public QuestionCategory()
        {
            AssignedQuestionCategories = new HashSet<AssignedQuestionCategory>();
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public Guid TestCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TestCategory TestCategory { get; set; }
        public virtual ICollection<AssignedQuestionCategory> AssignedQuestionCategories { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
