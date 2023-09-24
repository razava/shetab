using System.ComponentModel.DataAnnotations;

namespace Api.Authentication;

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public string VerificationCode { get; set; }
    public string RecaptchaToken { get; set; }
    public Guid CaptchaKey { get; set; }
    public string CaptchaValue { get; set; }
}

public class ForgotPasswordModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }
    public Guid CaptchaKey { get; set; }
    public string CaptchaValue { get; set; }
}
