namespace LodgeMasterWeb.Core.Models;

public class ColorCategory
{
    [Key]
    public int ColorId { get; set; }
    public string CompanyID { get; set; } = string.Empty;
    public string ColorValue { get; set; } = string.Empty;
    public int Sorted { get; set; } = 0;
}
