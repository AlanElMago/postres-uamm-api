using System.ComponentModel.DataAnnotations;

namespace PostresUAMMApi.Models.Forms;

public class UserLoginForm
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
