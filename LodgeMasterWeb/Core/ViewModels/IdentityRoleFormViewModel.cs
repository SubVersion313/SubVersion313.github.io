namespace LodgeMasterWeb.Core.ViewModels
{
    public class IdentityRoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
        //public string Description { get; set; } = string.Empty;
        //public int bActive { get; set; } = 0;
    }
}
