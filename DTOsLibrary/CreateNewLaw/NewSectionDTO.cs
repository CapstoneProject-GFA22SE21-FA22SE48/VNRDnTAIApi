using DTOsLibrary.CreateNewLaw;
using System;
using System.Collections.Generic;

namespace DTOsLibrary
{
    public class NewSectionDTO
    {
        public Guid StatueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid VehicleCategoryId { get; set; }
        public decimal MinPenalty { get; set; }
        public decimal MaxPenalty { get; set; }
        public bool IsSectionWithNoParagraph { get; set; }
        public List<ReferenceDTO> ReferenceParagraphs { get; set; }
        public List<NewParagraphDTO> Paragraphs { get; set; }

    }
}
