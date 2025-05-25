namespace LodgeMasterWeb.Core.Models
{
    public class TmSys_Service:BaseModel
    {
        [Key]
        public string SerivceID { get; set; }=string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string SerivceName_en { get; set; } = string.Empty;
        public string SerivceName_ar { get; set; } = string.Empty;
        public int iSorted { get; set; } = 0;

    }
}
