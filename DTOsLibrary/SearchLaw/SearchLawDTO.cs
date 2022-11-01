using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary.SearchLaw
{
    public class SearchLawDTO
    {
        public string Name { get; set; }
        public string StatueDesc { get; set; }
        public string SectionDesc { get; set; }
        public string? ParagraphDesc { get; set; }
        public string MinPenalty { get; set; }
        public string MaxPenalty { get; set; }
        public string AdditionalPenalty { get; set; }
        public List<SearchParagraphDTO> ReferenceParagraph { get; set; }

    }
}
