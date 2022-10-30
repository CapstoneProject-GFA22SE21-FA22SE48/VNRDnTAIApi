using System;

namespace DTOsLibrary.ManageROM
{
    public class QuestionRomDTO
    {
        public Guid ModifyingQuestionId { get; set; }
        public string ModifyingQuestionContent { get; set; }
        public Guid? ModifiedQuestionId { get; set; }
        public string ModifiedQuestionContent { get; set; }

        public Guid ScribeId { get; set; }
        public string Username { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeniedReason { get; set; }

        //Used for scribe rom
        public Guid AdminId { get; set; }
        public string AdminUsername { get; set; }
    }
}
