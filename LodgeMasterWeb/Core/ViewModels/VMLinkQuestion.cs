namespace LodgeMasterWeb.Core.ViewModels
{
    public class VMLinkQuestion
    {
        public string LinkPQID { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public string PartID { get; set; } = string.Empty;
        public string QuestionID { get; set; } = string.Empty;
        public string InspectionId { get; set; } = string.Empty;
        public string InspDepName { get; set; } = string.Empty;
        public string QuestionDisplay { get; set; } = string.Empty;
        public string InspName { get; set; } = string.Empty;
        public int DeleteInfos { get; set; } = 0;
        public int DeleteDeps { get; set; } = 0;
        public int DeleteQuestions { get; set; } = 0;
        public int isPublish { get; set; } = 0;
        public int SortPart { get; set; } = 0;
        public int SortQuestion { get; set; } = 0;
    }
}
