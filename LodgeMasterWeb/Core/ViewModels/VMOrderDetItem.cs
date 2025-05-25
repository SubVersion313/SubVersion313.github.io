//using DocumentFormat.OpenXml.Wordprocessing;

namespace LodgeMasterWeb.Core.ViewModels;

public class VMOrderDetItem
{
    public string OrderID { get; set; } = string.Empty;
    public string CompanyID { get; set; } = string.Empty;
    public string ItemID { get; set; } = string.Empty;
    public string sNotes { get; set; } = string.Empty;
    public string ItemName_E { get; set; } = string.Empty;
    public string ItemName_A { get; set; } = string.Empty;
    public int Qty { get; set; } = 0;

}
