namespace LodgeMasterWeb.Core.Models
{
    public class InspectionBasket
    {

        public int InspectionBasketId { get; set; }
        [MaxLength(250)]
        public string InspectionGUID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public DateTime dtEntry { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        [MaxLength(250)]
        public string LocationID { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string InspectionId { get; set; } = string.Empty;
        public string InspectionName { get; set; } = string.Empty;
        public string PartId { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string QuestionName { get; set; } = string.Empty;
        public int UserAnswer { get; set; } = 0;
        public string CommetAnswer { get; set; } = string.Empty;
        public string PicAnswer { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int CurrntQuestion { get; set; } = 0;
        public int SortQuestion { get; set; } = 0;
        public int QuestionNo { get; set; } = 0;
        public string QuestionTotal { get; set; } = string.Empty;

    }
}
