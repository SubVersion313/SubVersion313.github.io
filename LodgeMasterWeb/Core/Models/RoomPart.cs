namespace LodgeMasterWeb.Core.Models
{
    public class RoomPart : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string PartsID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string PartName { get; set; } = string.Empty;

    }
}
