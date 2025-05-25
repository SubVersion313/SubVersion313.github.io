namespace LodgeMasterWeb.Core.ViewModels
{
    public class VMInspDetails
    {
        public string InspectionID { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public string InspName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string CreateDate { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public int isClose { get; set; } = 0;
        public IEnumerable<VMInspQuestionDetails> LstQuestionData { get; set; } = Enumerable.Empty<VMInspQuestionDetails>();
    }

    public class VMInspQuestionDetails
    {
        public long DetailID { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string QuestionDisplay { get; set; } = string.Empty;
        public int UserAnswer { get; set; } = 0;
        public string CommetAnswer { get; set; } = string.Empty;
        public int UserAnswerAfter { get; set; } = 0;
        public string CommetAnswerAfter { get; set; } = string.Empty;
        public int iSorted { get; set; } = 0;
        public string PicBefore { get; set; } = string.Empty;
        public string PicBeforePath { get; set; } = string.Empty;
        public string PicAfter { get; set; } = string.Empty;
        public string PicAfterPath { get; set; } = string.Empty;

    }
}
