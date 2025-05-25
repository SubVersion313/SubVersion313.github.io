namespace LodgeMasterWeb.Core.ViewModels
{
    public class IdentityUserViewModel
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public string? RoleList { get; set; }
        public IEnumerable<SelectListItem> LstAllRoles { get; set; } = Enumerable.Empty<SelectListItem>();
        public string? CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public IEnumerable<SelectListItem> LstDepartment { get; set; } = Enumerable.Empty<SelectListItem>();
        public string? DepartmentName { get; set; }
        public int bActive { get; set; }
        public bool bActive2 { get; set; }
        public string? bPhoto { get; set; }
        public string? Photopath { get; set; }
        public int? expiredate { get; set; }
        public int supervisor { get; set; }
        public bool supervisor2 { get; set; } 
        public int? iSorted { get; set; }
        public int? IsDeleted { get; set; }
        public List<string> RoleSelectedValues { get; set; }


        //[AllowedExtensions(FileSettings.AllowedExtensions)
        //    , MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile? EmployeeImage { get; set; } = default!;

        //for hold data only
        //public string hd_Password { get; set; } = string.Empty;
        //public string hd_ConfirmdPassword { get; set; } = string.Empty;
        //public string hd_LoginName { get; set; } = string.Empty;
    }
}
