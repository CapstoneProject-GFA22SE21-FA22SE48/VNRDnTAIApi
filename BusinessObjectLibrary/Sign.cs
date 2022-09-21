using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Sign
    {
        public Sign()
        {
            Gpssigns = new HashSet<Gpssign>();
            SignParagraphs = new HashSet<SignParagraph>();
        }

        public Guid Id { get; set; }
        public Guid SignCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual SignCategory SignCategory { get; set; }
        public virtual ICollection<Gpssign> Gpssigns { get; set; }
        public virtual ICollection<SignParagraph> SignParagraphs { get; set; }
    }
}
