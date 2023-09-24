using System.ComponentModel.DataAnnotations;

namespace Api.Authentication;

public class RegisterModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    //[EmailAddress]
    //[Required(ErrorMessage = "Email is required")]
    //public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public string RecaptchaToken { get; set; }
    public Guid CaptchaKey { get; set; }
    public string CaptchaValue { get; set; }
}

public class RegisterWithRolesDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Organization { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IEnumerable<int> RegionIds { get; set; }
}
