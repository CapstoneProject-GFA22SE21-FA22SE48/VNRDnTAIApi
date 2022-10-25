using System;

namespace DTOsLibrary.ManageROM
{
    public class QuestionRomDTO
    {
        public Guid QuestionRomId { get; set; }
        public Guid ModifyingQuestionId { get; set; }
        public string ModifyingQuestionContent { get; set; }
        public Guid? ModifiedQuestionId { get; set; }
        public string ModifiedQuestionContent { get; set; }

        public Guid ScribeId { get; set; }
        public string Username { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
    }
}
