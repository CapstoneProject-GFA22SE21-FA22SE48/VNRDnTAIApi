using System;

namespace DTOsLibrary
{
    public class SignParagraphDTO
    {
        public Guid SignParagraphParagraphId { get; set; }
        public string SignParagraphParagraphName { get; set; }
        public string SignParagraphParagraphDesc { get; set; }
        public Guid SignParagraphSectionId { get; set; }
        public string SignParagraphSectionName { get; set; }
        public Guid SignParagraphStatueId { get; set; }
        public string SignParagraphStatueName { get; set; }
    }
}
