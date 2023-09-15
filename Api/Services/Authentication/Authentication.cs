using Domain.Models.Relational;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Api.Services.Authentication;

public class Authentication
{
    /*
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly TimeSpan _jwtExpiration;

    public Authentication(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _jwtSecret = "";
        _jwtIssuer = "";
        _jwtAudience = "";
        _jwtExpiration = new TimeSpan(365, 0, 0, 0);
    }

    private async Task<LoginResult> LoginUser(ApplicationUser user, string password)
    {
        var result = new LoginResult();
        if (await _userManager.CheckPasswordAsync(user, password))
        {
            result.Token = await generateToken(user);
            if (result.Token == null)
            {
                result.Error = "خطایی در ایجاد توکن رخ داد.";
            }
        }
        else
        {
            result.Error = "اطلاعات ورودی نادرست است.";
        }

        return result;
    }

    private async Task<string?> generateToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName??""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(AppClaimTypes.InstanceId, (user.ShahrbinInstanceId ?? -1).ToString())
                };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            expires: DateTime.Now.Add(_jwtExpiration),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }

    public async Task Register(ApplicationUser user, string password)
    {
        return;
    }

    public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
    {
        IdentityResult result;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            result.
        }
        else 
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            result.
        }
        
    }

    public async Task ResetPassword(ApplicationUser user, string newPassword)
    {
        return;
    }
    */
}

public class LoginResult
{
    public string? Token { get; set; }
    public string? Error { get; set; }
}