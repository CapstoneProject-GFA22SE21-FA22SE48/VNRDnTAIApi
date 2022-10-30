using System;
using System.Collections.Generic;

namespace DTOsLibrary.ManageROM
{
    public class LawRomDTO
    {
        public Guid LawRomId { get; set; }
        public Guid? ModifyingStatueId { get; set; }
        public string ModifyingStatueName { get; set; }
        public Guid? ModifiedStatueId { get; set; }
        public string ModifiedStatueName { get; set; }

        public Guid? ModifyingSectionId { get; set; }
        public string ModifyingSectionName { get; set; }
        public Guid? ModifiedSectionId { get; set; }
        public string ModifiedSectionName { get; set; }

        public Guid? ModifyingParagraphId { get; set; }
        public string ModifyingParagraphName { get; set; }
        public Guid? ModifiedParagraphId { get; set; }
        public string ModifiedParagraphName { get; set; }

        public Guid ScribeId { get; set; }
        public string Username { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<ReferenceDTO> ParagraphReferenceList { get; set; }
        public string DeniedReason { get; set; }

        //Used for scribe rom
        public Guid AdminId { get; set; }
        public string AdminUsername { get; set; }
    }
}
