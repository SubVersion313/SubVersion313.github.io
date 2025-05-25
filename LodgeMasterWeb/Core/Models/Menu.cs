namespace LodgeMasterWeb.Core.Models
{
    public class Menu
    {

        public int MenuID { get; set; } = 0;
        public int iParentID { get; set; } = 0;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public int bActive { get; set; } = 0;
        public int iSorted { get; set; } = 0;
        public string sURL { get; set; } = string.Empty;
        public string MenuText_A { get; set; } = string.Empty;
        public string MenuText_E { get; set; } = string.Empty;
        public string MenuText_Ben { get; set; } = string.Empty;
        public string MenuText_Nep { get; set; } = string.Empty;
        public string MenuText_Ind { get; set; } = string.Empty;
        public int iFormID { get; set; } = 0;
        public string sIcon { get; set; } = string.Empty;

    }
}
