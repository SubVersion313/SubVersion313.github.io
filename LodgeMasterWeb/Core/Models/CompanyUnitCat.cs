namespace LodgeMasterWeb.Core.Models;

public class CompanyUnitCat : BaseModel
{
    [Key]
    [MaxLength(250)]
    public string UnitCatId { get; set; }
    public string CompanyID { get; set; }
    public string UnitCatName { get; set; }
    public string UnitCatDescription { get; set; } = string.Empty;
    public string UnitCatColor { get; set; } = string.Empty;

}
