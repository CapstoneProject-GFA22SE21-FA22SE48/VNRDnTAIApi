using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Paragraph
    {
        public Paragraph()
        {
            Keywords = new HashSet<Keyword>();
            ParagraphModificationRequests = new HashSet<ParagraphModificationRequest>();
            ReferenceParagraphs = new HashSet<Reference>();
            ReferenceReferenceParagraphs = new HashSet<Reference>();
            SignParagraphs = new HashSet<SignParagraph>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SectionId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Section Section { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
        public virtual ICollection<ParagraphModificationRequest> ParagraphModificationRequests { get; set; }
        public virtual ICollection<Reference> ReferenceParagraphs { get; set; }
        public virtual ICollection<Reference> ReferenceReferenceParagraphs { get; set; }
        public virtual ICollection<SignParagraph> SignParagraphs { get; set; }
    }
}
