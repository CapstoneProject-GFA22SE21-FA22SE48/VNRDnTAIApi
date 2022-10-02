using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ParagraphModificationRequest
    {
        public Guid ModifiedParagraphId { get; set; }
        public Guid ModifyingParagraphId { get; set; }
        public Guid ScribeId { get; set; }
        public Guid AdminId { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Admin { get; set; }
        public virtual Paragraph ModifiedParagraph { get; set; }
        public virtual Paragraph ModifyingParagraph { get; set; }
        public virtual User Scribe { get; set; }
    }
}
