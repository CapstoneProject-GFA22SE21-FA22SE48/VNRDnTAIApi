using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class KeywordParagraph
    {
        public Guid KeywordId { get; set; }
        public Guid ParagraphId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Paragraph Paragraph { get; set; }
    }
}
