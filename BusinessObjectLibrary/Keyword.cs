using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Keyword
    {
        public Keyword()
        {
            KeywordParagraphs = new HashSet<KeywordParagraph>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<KeywordParagraph> KeywordParagraphs { get; set; }
    }
}
