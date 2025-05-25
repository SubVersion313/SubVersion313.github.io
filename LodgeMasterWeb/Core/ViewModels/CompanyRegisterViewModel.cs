namespace LodgeMasterWeb.Core.ViewModels;

public class CompanyRegisterViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Company is required")]
    [MaxLength(250)]
    public string CompanyName_En { get; set; }
    [MaxLength(250)]
    public string CompanyName_Ar { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Personal name is required")]
    public string PersonalName { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    public string Phonenumber { get; set; }

    public string Address { get; set; }

    public int StatusOrder { get; set; } = 0;

    public DateTime OrderDate { get; set; }

}
