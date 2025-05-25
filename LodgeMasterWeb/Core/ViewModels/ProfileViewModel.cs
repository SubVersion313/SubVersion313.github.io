namespace LodgeMasterWeb.Core.ViewModels
{
    public class ProfileViewModel
    {
        public string EmpID { get; set; } = string.Empty;
        public string UserLogin { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public string sEmail { get; set; } = string.Empty;
        public string bPhoto { get; set; } = string.Empty;
        public string Photopath { get; set; } = string.Empty;
        public string CompanyFolder { get; set; } = string.Empty;

        public string mobile { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        //public IFormFile physImage { get; set; }

    }
}
