using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class SignParagraph
    {
        public Guid Id { get; set; }
        public Guid SignId { get; set; }
        public Guid ParagraphId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Paragraph Paragraph { get; set; }
        public virtual Sign Sign { get; set; }
    }
}
