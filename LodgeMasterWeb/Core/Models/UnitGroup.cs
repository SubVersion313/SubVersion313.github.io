namespace LodgeMasterWeb.Core.Models
{
    public class UnitGroup : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string GroupID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string GroupName { get; set; } = string.Empty;

    }
}
