namespace LodgeMasterWeb.Core.Models
{
    public class sys_Menu
    {
        [Key]
        public int MenuID { get; set; }
        public int iParentID { get; set; }
        public int bActive { get; set; }
        public int iSorted { get; set; }
        public string sURL { get; set; } = string.Empty;
        public string MenuText_A { get; set; } = string.Empty;
        public string MenuText_E { get; set; } = string.Empty;
        public string MenuText_Ben { get; set; } = string.Empty;
        public string MenuText_Nep { get; set; } = string.Empty;
        public string MenuText_Ind { get; set; } = string.Empty;
        public int iFormID { get; set; }
        public string sIcon { get; set; } = string.Empty;
    }
}
