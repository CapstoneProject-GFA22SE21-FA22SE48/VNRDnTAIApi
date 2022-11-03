using BusinessObjectLibrary;
using System;
using System.Collections.Generic;

namespace DTOsLibrary
{
    public class SignDTO
    {
        public Guid Id { get; set; }
        public Guid SignCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public SignCategory SignCategory { get; set; }

        public List<SignParagraphDTO> SignParagraphs { get; set; }

    }
}
