namespace LodgeMasterWeb.Core.ViewModels
{
    public class IdentityPermissionsFormVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<IdentityCheckBoxViewModel> RoleCalims { get; set; }
    }
}
