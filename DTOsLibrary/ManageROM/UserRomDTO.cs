using System;

namespace DTOsLibrary.ManageROM
{
    public class UserRomDTO
    {
        public Guid ModifyingUserId { get; set; }
        public string ModfifyingUserName { get; set; }

        public Guid PromotingAdminId { get; set; }
        public string PromotingAdminUsername { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeniedReason { get; set; }

    }
}
