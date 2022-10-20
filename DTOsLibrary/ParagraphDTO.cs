using BusinessObjectLibrary;
using System;
using System.Collections.Generic;

namespace DTOsLibrary
{
    public class ParagraphDTO
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string AdditionalPenalty { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? KeywordId { get; set; }

        public virtual Section Section { get; set; }
        public virtual List<ReferenceDTO> ReferenceParagraphs { get; set; }

    }
}
