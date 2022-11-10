using System;

namespace DTOsLibrary.ManageROM
{
    public class SignRomDTO
    {
        public Guid SignRomId { get; set; }
        public Guid? ModifyingSignId { get; set; }
        public string ModifyingSignName { get; set; }
        public Guid? ModifiedSignId { get; set; }
        public string ModifiedSignName { get; set; }

        //admin only handle ROM from scribe, ROM from handle will be handled by scribe
        //public Guid? UserId { get; set; } 
        public Guid? ScribeId { get; set; }
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
