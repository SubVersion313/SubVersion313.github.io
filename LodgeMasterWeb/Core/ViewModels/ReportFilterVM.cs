namespace LodgeMasterWeb.Core.ViewModels
{
    public class ReportFilterVM
    {

        public int isComeFrom { get; set; } = 0;
        public int filterPeriod { get; set; } = 0;
        public string? filterDateFrom { get; set; } = string.Empty;
        public string? filterDateTo { get; set; } = string.Empty;

    }
}
