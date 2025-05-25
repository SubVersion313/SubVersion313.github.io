namespace LodgeMasterWeb.Core.Models
{
    public class sys_Status
    {
        [Key]
        public int StatusID { get; set; }
        [MaxLength(250)]
        public string Status_E { get; set; } = string.Empty;
        [MaxLength(250)]
        public string Status_A { get; set; } = string.Empty;
        public int StatusActive { get; set; }
        public int SatausParent { get; set; }
        public int StatusSortShow { get; set; }
    }
}
