using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class Paragraph
    {
        public Paragraph()
        {
            KeywordParagraphs = new HashSet<KeywordParagraph>();
            LawModificationRequestModifiedParagraphs = new HashSet<LawModificationRequest>();
            LawModificationRequestModifyingParagraphs = new HashSet<LawModificationRequest>();
            ReferenceParagraphs = new HashSet<Reference>();
            ReferenceReferenceParagraphs = new HashSet<Reference>();
            SignParagraphs = new HashSet<SignParagraph>();
        }

        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string AdditionalPenalty { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Section Section { get; set; }
        public virtual ICollection<KeywordParagraph> KeywordParagraphs { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestModifiedParagraphs { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestModifyingParagraphs { get; set; }
        public virtual ICollection<Reference> ReferenceParagraphs { get; set; }
        public virtual ICollection<Reference> ReferenceReferenceParagraphs { get; set; }
        public virtual ICollection<SignParagraph> SignParagraphs { get; set; }
    }
}
