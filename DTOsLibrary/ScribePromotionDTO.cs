using BusinessObjectLibrary;
using System;

namespace DTOsLibrary
{
    public class ScribePromotionDTO
    {
        public Guid ScribeId { get; set; }
        public Guid PromotingAdminId { get; set; }
        public Guid ArbitratingAdminId { get; set; }
        public string ErrorMessage { get; set; }

        //used for notification
        public User Scribe { get; set; }
        public User PromotingAdmin { get; set; }
        public User ArbitratingAdmin { get; set; }

    }
}
