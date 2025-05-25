namespace LodgeMasterWeb.Core.ViewModels
{
    public class InspectionWizardViewModel
    {
        public int InspectionBasketId { get; set; }
        public string InspectionGUID { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public DateTime dtEntry { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public string LocationID { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
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

        public IEnumerable<VMDisplayPart> LstParts { get; set; } = Enumerable.Empty<VMDisplayPart>();

    }

    public class VMDisplayPart
    {
        public int PartNo { get; set; }
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int PartActive { get; set; }
        public int PartQuestionNo { get; set; }


    }
}
