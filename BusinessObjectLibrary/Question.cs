using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public Guid Id { get; set; }
        public Guid TestCategoryId { get; set; }
        public string Name { get; set; }
        public string Question1 { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TestCategory TestCategory { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
