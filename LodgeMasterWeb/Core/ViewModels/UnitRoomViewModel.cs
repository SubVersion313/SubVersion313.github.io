namespace LodgeMasterWeb.Core.ViewModels;

public class UnitRoomViewModel
{

    [MaxLength(450)]
    public string UnitId { get; set; }
    public string UnitName { get; set; }
    public string CompanyID { get; set; }
    public int AreaTypeId { get; set; }
    public string UnitTypeId { get; set; }
    public string FloorID { get; set; }
    [MaxLength(250)]
    public string UnitDescription { get; set; } = string.Empty;
    public List<SelectListItem> ListAreaTypeName { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> ListUnitTypeName { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> ListFloorName { get; set; } = Enumerable.Empty<SelectListItem>();


}
