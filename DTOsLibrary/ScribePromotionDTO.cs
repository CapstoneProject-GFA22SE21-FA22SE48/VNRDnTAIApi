using System;

namespace DTOsLibrary
{
    public class ScribePromotionDTO
    {
        public Guid ScribeId { get; set; }
        public Guid PromotingAdminId { get; set; }
        public Guid ArbitratingAdminId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
