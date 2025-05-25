namespace LodgeMasterWeb.Core.Models;

public class CompanyUnitCatType : BaseModel
{
    [Key]
    [MaxLength(250)]
    public string UnitCatTypeId { get; set; }
    public string CompanyID { get; set; }
    public string UnitCatTypeName { get; set; }



}
