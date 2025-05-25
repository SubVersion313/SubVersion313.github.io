namespace LodgeMasterWeb.Core.Models
{
    public class UnitType : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string UnitID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string UnitName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;

    }
}
