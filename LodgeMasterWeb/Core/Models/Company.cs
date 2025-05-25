

namespace LodgeMasterWeb.Core.Models
{
    public class Company
    {
        [Key]
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyName_E { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyName_A { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Companylogin { get; set; } = string.Empty;
        [MaxLength(250)]
        public string MasterEmail { get; set; } = string.Empty;
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;
        [MaxLength(50)]
        public string PersonName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Mobile { get; set; } = string.Empty;
        [MaxLength(250)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string CompanyFolder { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public DateTime CreateCompany { get; set; } = GeneralFun.GetCurrentTime();// DateTime.Now;
        public long StartDate { get; set; } = 0;
        public long EndDate { get; set; } = 0;
        public int CounterUsers { get; set; } = 0;
        public int isDemo { get; set; } = 0;
        public int isDeleted { get; set; } = 0;
        public int GenerateRoom { get; set; } = 0;
        [MaxLength(250)]
        public string CompanyNameLogin { get; set; } = string.Empty;
    }
}
