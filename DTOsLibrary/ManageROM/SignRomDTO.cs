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

        public Guid? ModifyingGpssignId { get; set; }
        public string ModifyingGpssignName { get; set; }
        public Guid? ModifiedGpssignId { get; set; }
        public string ModifiedGpssignName { get; set; }

        public Guid? UserId { get; set; }
        public Guid? ScribeId { get; set; }
        public string Username { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
