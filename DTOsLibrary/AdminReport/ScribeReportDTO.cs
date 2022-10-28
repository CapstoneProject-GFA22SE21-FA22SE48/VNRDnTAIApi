namespace DTOsLibrary.AdminReport
{
    public class ScribeReportDTO
    {
        public int TotalScribeCount { get; set; }
        public int NewScribeByMonthYearCount { get; set; }
        public int ActiveScribeCount { get; set; }
        public int DeactivatedScribeCount { get; set; }
    }
}
