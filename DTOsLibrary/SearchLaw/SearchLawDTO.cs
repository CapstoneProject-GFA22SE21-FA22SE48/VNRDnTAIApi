using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary.SearchLaw
{
    public class SearchLawDTO
    {
        public String SectionDesc { get; set; }
        public String? ParagraphDesc { get; set; }
        public String MinPenalty { get; set; }
        public String MaxPenalty { get; set; }
        public String AdditionalPenalty { get; set; }

    }
}
