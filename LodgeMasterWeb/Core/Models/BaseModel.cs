namespace LodgeMasterWeb.Core.Models
{
    public class BaseModel
    {


        public string CreateEmpID { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public int bActive { get; set; }
        public int IsDeleted { get; set; }
        public string DeleteEmpID { get; set; } = string.Empty;
    }
}
