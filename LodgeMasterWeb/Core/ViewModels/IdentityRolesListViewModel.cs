namespace LodgeMasterWeb.Core.ViewModels
{
    public class IdentityRolesListViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityCheckBoxViewModel> Roles { get; set; }
    }
}
