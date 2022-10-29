using System;

namespace DTOsLibrary
{
    public class ScribeDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        //Additional fields
        public int TotalRequestCountByMonthYear { get; set; }
        public int PendingRequestCountByMonthYear { get; set; }
        public int ApprovedRequestCountByMonthYear { get; set; }
        public int DeniedRequestCountByMonthYear { get; set; }
        public int ConfirmedRequestCountByMonthYear { get; set; }
        public int CancelledRequestCountByMonthYear { get; set; }

    }
}
