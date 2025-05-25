namespace LodgeMasterWeb.Core.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Organization { get; set; } = string.Empty;
        [Required]
        public string Loginemail { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool Remember { get; set; } = false;

    }
}
