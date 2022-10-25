using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary.SearchLaw
{
    public class SearchParagraphDTO
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string AdditionalPenalty { get; set; }
        public string MinPenalty { get; set; }
        public string MaxPenalty { get; set; }
        public bool IsDeleted { get; set; }
        public List<SearchParagraphDTO>? ReferenceParagraph { get; set; }

    }
}
