namespace LodgeMasterWeb.Core.Models
{
    public class TempFile
    {
        [Key]
        [MaxLength(250)]
        public string FileID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string OrderID { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string CreateEmpID { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FileType { get; set; } = string.Empty;
        public int bShow { get; set; }
        public int bStatus { get; set; }
    }
}
