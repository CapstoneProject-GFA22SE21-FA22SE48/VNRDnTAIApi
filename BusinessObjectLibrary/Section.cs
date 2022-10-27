using System;
using System.Collections.Generic;

namespace BusinessObjectLibrary
{
    public partial class Section
    {
        public Section()
        {
            LawModificationRequestModifiedSections = new HashSet<LawModificationRequest>();
            LawModificationRequestModifyingSections = new HashSet<LawModificationRequest>();
            Paragraphs = new HashSet<Paragraph>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid VehicleCategoryId { get; set; }
        public Guid StatueId { get; set; }
        public string Description { get; set; }
        public decimal MinPenalty { get; set; }
        public decimal MaxPenalty { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Statue Statue { get; set; }
        public virtual VehicleCategory VehicleCategory { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestModifiedSections { get; set; }
        public virtual ICollection<LawModificationRequest> LawModificationRequestModifyingSections { get; set; }
        public virtual ICollection<Paragraph> Paragraphs { get; set; }
    }
}
