using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class AssignedQuestionCategory
    {
        public Guid QuestionCategoryId { get; set; }
        public Guid ScribeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual User Scribe { get; set; }
    }
}
