using Microsoft.AspNetCore.Identity;

namespace LodgeMasterWeb.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepartmentID { get; set; } = string.Empty;
        //[ForeignKey(nameof(DepartmentID))]
        //public Department DepartmentData { get; set; }


        public string CreateEmpID { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        //public byte[] bytePhoto { get; set; } //= null;
        public string bPhoto { get; set; } = string.Empty;
        public string Photopath { get; set; } = string.Empty;

        public int expiredate { get; set; } = 0;
        public int supervisor { get; set; } = 0;
        public DateTime dtpasswordupdate { get; set; } = GeneralFun.GetCurrentTime(); //
        public int iSorted { get; set; }
        public int IsDeleted { get; set; } = 0;

        public ICollection<WhatsappBotAction>? WhatsappBotActions { get; set; }

    }
}
