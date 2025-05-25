namespace LodgeMasterWeb.Core.Models
{
    public class RoomLinkTypeComUnit
    {
        [Key]
        [MaxLength(250)]
        public string LinkTCUID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string LocationID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string RoomTypeID { get; set; } = string.Empty;
    }
}
