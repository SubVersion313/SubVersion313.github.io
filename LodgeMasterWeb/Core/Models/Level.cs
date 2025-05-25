namespace LodgeMasterWeb.Core.Models
{
    public class Level : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string LevelID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string LevelName_EN { get; set; } = string.Empty;
        [MaxLength(250)]
        public string LevelName_AR { get; set; } = string.Empty;
        public int isDefault { get; set; } = 0;

    }
}
