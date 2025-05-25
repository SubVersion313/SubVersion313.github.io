namespace LodgeMasterWeb.Core.Models
{
    public class sys_Problem
    {
        [Key]
        [MaxLength(250)]
        public string iProblem_Code { get; set; } = string.Empty;
        public string sProblem_Description_E { get; set; } = string.Empty;
        public string sProblem_Description_A { get; set; } = string.Empty;
        public string sProblem_Description_Ben { get; set; } = string.Empty;
        public string sProblem_Description_Nep { get; set; } = string.Empty;
        public string sProblem_Description_Ind { get; set; } = string.Empty;
        public string sRecommended_Solution_E { get; set; } = string.Empty;
        public string sRecommended_Solution_A { get; set; } = string.Empty;
        public string sRecommended_Solution_Ben { get; set; } = string.Empty;
        public string sRecommended_Solution_Nep { get; set; } = string.Empty;
        public string sRecommended_Solution_Ind { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public int iSorted { get; set; } = 0;
    }
}
