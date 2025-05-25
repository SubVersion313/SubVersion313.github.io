namespace LodgeMasterWeb.Core.Models
{
    public class RoomType : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string RoomTypeID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string TypeName { get; set; } = string.Empty;

    }
}
