using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Question Question { get; set; }
    }
}
