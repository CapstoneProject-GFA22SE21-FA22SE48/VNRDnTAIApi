using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Keyword
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParagraphId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Paragraph Paragraph { get; set; }
    }
}
