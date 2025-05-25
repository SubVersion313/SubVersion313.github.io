
namespace LodgeMasterWeb.Core.Models
{
    public class InspectionDet
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long DetailID { get; set; }
        [MaxLength(250)]
        public string InspectionID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string QuestionID { get; set; } = string.Empty;
        public string QuestionDisplay { get; set; } = string.Empty;
        public string PartID { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public int iSorted { get; set; }
        public int UserAnswer { get; set; }
        public string CommetAnswer { get; set; } = string.Empty;
        public int UserAnswerAfter { get; set; }
        public string CommetAnswerAfter { get; set; } = string.Empty;
        public string PicBefore { get; set; } = string.Empty;
        public DateTime PicBeforCreate { get; set; } = GeneralFun.GetCurrentTime(); //
        public string PicAfter { get; set; } = string.Empty;
        public DateTime PicAfterCreate { get; set; } = GeneralFun.GetCurrentTime();
        public int bDone { get; set; } = 0;
        public DateTime DoneCreate { get; set; } = GeneralFun.GetCurrentTime(); //
    }
}
