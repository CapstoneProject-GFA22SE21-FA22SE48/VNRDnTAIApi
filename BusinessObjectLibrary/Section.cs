using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Section
    {
        public Section()
        {
            Paragraphs = new HashSet<Paragraph>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid StatueId { get; set; }
        public string Description { get; set; }
        public decimal? MinPenalty { get; set; }
        public decimal? MaxPenalty { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Statue Statue { get; set; }
        public virtual ICollection<Paragraph> Paragraphs { get; set; }
    }
}
