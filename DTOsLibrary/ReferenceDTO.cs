using System;

namespace DTOsLibrary
{
    public class ReferenceDTO
    {
        public Guid ReferenceParagraphId { get; set; }
        public string ReferenceParagraphName { get; set; }
        public string ReferenceParagraphDesc { get; set; }
        public Guid ReferenceParagraphSectionId { get; set; }
        public string ReferenceParagraphSectionName { get; set; }
        public Guid ReferenceParagraphSectionStatueId { get; set; }
        public string ReferenceParagraphSectionStatueName { get; set; }
        public bool ReferenceParagraphIsExcluded { get; set; }
    }
}
