namespace LodgeMasterWeb.Core.Models;

public class CompanyUnitLinkCatType
{
    [Key]
    [MaxLength(250)]
    public string LinkCartypeAndCatID { get; set; } = string.Empty;

    public string CompanyID { get; set; } = string.Empty;

    public string UnitCatId { get; set; } = string.Empty;
    public string UnitCatTypeId { get; set; } = string.Empty;
}
