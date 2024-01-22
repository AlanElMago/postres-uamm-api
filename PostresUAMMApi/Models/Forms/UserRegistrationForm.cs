using System.ComponentModel.DataAnnotations;

namespace PostresUAMMApi.Models.Forms;

public class UserRegistrationForm
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string? FullName { get; set; }
}
