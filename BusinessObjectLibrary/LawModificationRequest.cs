using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class LawModificationRequest
    {
        public Guid Id { get; set; }
        public Guid? ModifyingStatueId { get; set; }
        public Guid? ModifiedStatueId { get; set; }
        public Guid? ModifyingSectionId { get; set; }
        public Guid? ModifiedSectionId { get; set; }
        public Guid? ModifyingParagraphId { get; set; }
        public Guid? ModifiedParagraphId { get; set; }
        public Guid ScribeId { get; set; }
        public Guid AdminId { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Admin { get; set; }
        public virtual Paragraph ModifiedParagraph { get; set; }
        public virtual Section ModifiedSection { get; set; }
        public virtual Statue ModifiedStatue { get; set; }
        public virtual Paragraph ModifyingParagraph { get; set; }
        public virtual Section ModifyingSection { get; set; }
        public virtual Statue ModifyingStatue { get; set; }
        public virtual User Scribe { get; set; }
    }
}
