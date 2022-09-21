using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ParagraphModificationRequest
    {
        public Guid ParagraphId { get; set; }
        public Guid UserId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Paragraph Paragraph { get; set; }
        public virtual User User { get; set; }
    }
}
