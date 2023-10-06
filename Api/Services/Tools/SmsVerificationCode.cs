using Domain.Models.Relational.IdentityAggregate;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Tools;

public static class SMSVerificationCode
{
    public static async Task<string> GenerateToken(UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.GenerateAsync("Phone number verification", userManager, user);
    }

    public static async Task<bool> ValidateToken(string token, UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.ValidateAsync("Phone number verification", token, userManager, user);
    }
}
