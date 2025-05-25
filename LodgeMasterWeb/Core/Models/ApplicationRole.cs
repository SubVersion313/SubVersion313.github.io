using Microsoft.AspNetCore.Identity;

namespace LodgeMasterWeb.Core.Models
{
    public class ApplicationRole : IdentityRole
    {
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public int isDefault { get; set; } = 1;
        public int isDeleted { get; set; } = 0;


    }
}
