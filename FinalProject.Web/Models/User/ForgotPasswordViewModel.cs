using System.ComponentModel.DataAnnotations;

namespace FinalProject.Web.Models.User;
public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
}
