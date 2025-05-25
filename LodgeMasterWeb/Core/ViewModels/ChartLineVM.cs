namespace LodgeMasterWeb.Core.ViewModels;

public class ChartLineVM
{

    public String TimePeriod { get; set; }=string.Empty;
    public int CountData { get; set; } = 0;
}
public class ChartLineWeekVM
{

    public String WeekDay { get; set; } = string.Empty;
    public String FormattedDate { get; set; } = string.Empty;
    public int CountData { get; set; } = 0;
}
public class ChartLineMonthVM
{
    public int YearDate { get; set; } = 0;
    public int MonthDate { get; set; } = 0;
    public int WeekNumber { get; set; } = 0;
    public int CountData { get; set; } = 0;
}