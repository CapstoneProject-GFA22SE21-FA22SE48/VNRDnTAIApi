namespace DTOsLibrary.AdminReport
{
    public class RomReportDTO
    {
        public int TotalRomCount { get; set; }
        public int PendingRomCount { get; set; }
        public int ApprovedRomCount { get; set; }
        public int DeniedRomCount { get; set; }
        public int ConfirmedRomCount { get; set; }
        public int CancelledRomCount { get; set; }
    }
}
