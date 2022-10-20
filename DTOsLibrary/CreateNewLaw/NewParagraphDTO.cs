using System;
using System.Collections.Generic;

namespace DTOsLibrary.CreateNewLaw
{
    public class NewParagraphDTO
    {
        public Guid? SectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalPenalty { get; set; }
        public Guid? KeywordId { get; set; }
        public List<ReferenceDTO> ReferenceParagraphs { get; set; }
    }
}
