
namespace LodgeMasterWeb.Core.Models
{
    public class sys_Item
    {
        [Key]
        public int ItemID { get; set; }
        [MaxLength(250)]
        public string ItemName_E { get; set; } = string.Empty;
        [MaxLength(250)]
        public string ItemName_A { get; set; } = string.Empty;
        public int ItemType { get; set; }
        public int bActive { get; set; } = 1;
        public string DefaultID { get; set; } = string.Empty;
    }
}
