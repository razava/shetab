﻿using Application.Common.Interfaces.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using FluentResults;
using SharedKernel.Errors;
using Application.Common.Statics;
using Domain.Models.Relational.IdentityAggregate;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.MyYazd;
using Domain.Models.MyYazd;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using Application.Common.Interfaces.Persistence;

namespace Infrastructure.Authentication;

public class AuthenticationService(
    UserManager<ApplicationUser> userManager,
    JwtInfo jwtInfo,
    TokenValidationParameters tokenValidationParameters,
    IAuthenticateRepository authenticateRepository,
    ICommunicationService communicationService,
    IMyYazdService myYazdService,
    IStorageService storageService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IAuthenticationService
{
    public async Task<Result<LoginResultModel>> Login(
        string username,
        string password)
    {
        username = username.Trim();
        var result = await GetUser(username);
        if (result.IsFailed)
            return result.ToResult();

        var user = result.Value;
        if (await userManager.CheckPasswordAsync(user, password))
        {
            if (user.TwoFactorEnabled)
            {
                var verificationCodeResult = await sendVerificationCode(user);
                if (verificationCodeResult.IsFailed)
                    return verificationCodeResult.ToResult();
                return new LoginResultModel(null, verificationCodeResult.Value);
            }
            else
            {
                return new LoginResultModel(await GenerateToken(user), null);
            }
        }
        else
        {
            var goldenUsers = await userManager.GetUsersInRoleAsync(RoleNames.GoldenUser);
            goldenUsers = goldenUsers.Where(u => u.ShahrbinInstanceId == user.ShahrbinInstanceId).ToList();

            foreach (var goldenUser in goldenUsers)
            {
                if (await userManager.CheckPasswordAsync(goldenUser, password))
                {
                    if (goldenUser.TwoFactorEnabled)
                    {
                        var verificationCodeResult = await sendVerificationCode(goldenUser, false, user);
                        if (verificationCodeResult.IsFailed)
                            return verificationCodeResult.ToResult();
                        return new LoginResultModel(null, verificationCodeResult.Value);
                    }
                    else
                    {
                        return new LoginResultModel(await GenerateToken(user), null);
                    }
                }
            }
            
            return AuthenticationErrors.InvalidCredentials;
        }
    }

    public async Task<Result<AuthToken>> VerifyOtp(string otpToken, string code)
    {
        var storedOtp = await authenticateRepository.GetOtpAsync(otpToken);
        if (storedOtp is null)
            return AuthenticationErrors.InvalidOtp;

        if (await ValidateOtp(storedOtp.User, code))
        {
            await authenticateRepository.DeleteOtpAsync(otpToken);
            if (storedOtp.IsNew)
            {
                await createCitizen(storedOtp.User);
            }
            return await GenerateToken(storedOtp.LoginAs ?? storedOtp.User);
        }

        return AuthenticationErrors.InvalidOtp;
    }

    public async Task<Result<VerificationToken>> LogisterCitizen(string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();
        if (!IsPhoneNumberValid(phoneNumber))
        {
            return AuthenticationErrors.InvalidUsername;
        }
        var user = await userManager.FindByNameAsync(phoneNumber);
        var isNew = user is null;
        if (user is null)
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = true,
                Title = "شهروند"
            };
        var verificationCodeResult = await sendVerificationCode(user, isNew);
        if (verificationCodeResult.IsFailed)
            return verificationCodeResult.ToResult();
        return verificationCodeResult.Value;
    }

    public async Task<Result<AuthToken>> LoginMyYazd(string code)
    {
        var userInfoResult = await myYazdService.GetUserInfo(code);
        if (userInfoResult.IsFailed)
        {
            return userInfoResult.ToResult();
        }

        var userInfo = userInfoResult.Value;
        if (!IsPhoneNumberValid(userInfo?.User?.MobileNo))
        {
            return AuthenticationErrors.InvalidUsername;
        }
        string phoneNumber = userInfo!.User!.MobileNo!;
        var user = await userManager.FindByNameAsync(phoneNumber);
        var isNew = user is null;
        if (user is null)
        {
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = true,
                Title = "شهروند",
            };

            await createCitizen(user);
        }
        await updateUserInfoFromYazdSso(user, userInfo.User);
        return await GenerateToken(user);
    }

    public async Task<Result<AuthToken>> Refresh(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);
        if (validatedToken == null)
        {
            return AuthenticationErrors.InvalidAccessToken;
        }

        var now = DateTime.UtcNow;
        var expiryDateUnix =
            long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);
        if (expiryDateUtc > now)
        {
            return AuthenticationErrors.TokenNotExpiredYet;
        }

        var storedRefreshToken = await authenticateRepository.GetRefreshTokenAsync(refreshToken);
        if (storedRefreshToken is null)
            return AuthenticationErrors.InvalidRefereshToken;

        // We don't need this when using Redis but is kept for compatibility
        if (now > storedRefreshToken.CreationDate.Add(storedRefreshToken.Expiry))
            return AuthenticationErrors.InvalidRefereshToken;

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (storedRefreshToken.JwtId != jti)
            return AuthenticationErrors.InvalidRefereshToken;

        await authenticateRepository.DeleteRefreshTokenAsync(refreshToken);

        var userName = validatedToken.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
            return AuthenticationErrors.InvalidUsername;

        var result = await GetUser(userName);
        if (result.IsFailed)
            return result.ToResult();
        return await GenerateToken(result.Value);
    }

    public async Task<Result<bool>> Revoke(string userId, string refreshToken)
    {
        var storedRefreshToken = await authenticateRepository.GetRefreshTokenAsync(refreshToken);
        if (storedRefreshToken is null)
            return AuthenticationErrors.InvalidRefereshToken;

        if (storedRefreshToken.UserId != userId)
            return AuthenticationErrors.InvalidRefereshToken;
        await authenticateRepository.DeleteRefreshTokenAsync(refreshToken);

        return true;
    }

    public async Task<Result<bool>> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var userResult = await GetUser(username);
        if (userResult.IsFailed)
            return userResult.ToResult();
        var user = userResult.Value;

        var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<Result<RequestToChangePhoneNumberResult>> RequestToChangePhoneNumber(
        string userName, string newPhoneNumber)
    {
        if (!IsPhoneNumberValid(newPhoneNumber))
        {
            return AuthenticationErrors.InvalidPhoneNumber;
        }
        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
            return AuthenticationErrors.UserNotFound;

        VerificationToken token1;
        if (!IsPhoneNumberValid(user.PhoneNumber))
        {
            token1 = new VerificationToken("", "");
        }
        else
        {
            var token1Result = await sendVerificationCode(user, false);
            if (token1Result.IsFailed)
                return token1Result.ToResult();
            token1 = token1Result.Value;
        }
        

        var tmpUser = new ApplicationUser()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = newPhoneNumber,
            PhoneNumber = newPhoneNumber,
            PhoneNumberConfirmed = false
        };
        var token2Result = await sendVerificationCode(tmpUser, true);
        if (token2Result.IsFailed)
        {
            return token2Result.ToResult();
        }

        return new RequestToChangePhoneNumberResult(token1, token2Result.Value);
    }

    public async Task<Result<bool>> ChangePhoneNumber(
        string userName, string otpToken1, string code1, string otpToken2, string code2)
    {
        var userResult = await GetUser(userName);
        if (userResult.IsFailed)
            return AuthenticationErrors.UserNotFound;
        var user = userResult.Value;

        
        if (IsPhoneNumberValid(user.PhoneNumber))
        {
            var storedOtp1 = await authenticateRepository.GetOtpAsync(otpToken1);
            if (storedOtp1 is null)
                return AuthenticationErrors.InvalidOtp;

            if (await ValidateOtp(storedOtp1.User, code1))
            {
                await authenticateRepository.DeleteOtpAsync(otpToken1);
            }
            else
            {
                return AuthenticationErrors.InvalidOtp;
            }
        }

        var storedOtp2 = await authenticateRepository.GetOtpAsync(otpToken2);
        if (storedOtp2 is null)
            return AuthenticationErrors.InvalidOtp;

        if (await ValidateOtp(storedOtp2.User, code2))
        {
            await authenticateRepository.DeleteOtpAsync(otpToken2);
        }
        else
        {
            return AuthenticationErrors.InvalidOtp;
        }

        user.PhoneNumber = storedOtp2.User.PhoneNumber;
        await userManager.UpdateAsync(user);

        return true;
    }

    public async Task<Result<VerificationToken>> GetResetPasswordToken(string username)
    {
        var user = await GetUser(username);
        if (user.IsFailed)
            return user.ToResult();

        if (!IsPhoneNumberValid(user.Value.PhoneNumber))
            return AuthenticationErrors.InvalidPhoneNumber;

        var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user.Value);
        if (passwordResetToken is null)
        {
            return AuthenticationErrors.ResetPasswordFailed;
        }

        var tokenResult = await sendVerificationCode(user.Value, false);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult();

        await authenticateRepository.InsertResetPasswordTokenAsync(
            new ResetPasswordToken(user.Value.Id, passwordResetToken));

        return tokenResult;
    }

    public async Task<Result<bool>> ResetPassword(string otpToken, string code, string newPassword)
    {
        var storedOtp = await authenticateRepository.GetOtpAsync(otpToken);
        if (storedOtp is null)
            return AuthenticationErrors.InvalidOtp;

        if (await ValidateOtp(storedOtp.User, code))
        {
            await authenticateRepository.DeleteOtpAsync(otpToken);
        }
        else
        {
            return AuthenticationErrors.InvalidOtp;
        }

        var resetPasswordToken = await authenticateRepository
            .GetResetPasswordTokenAsync(storedOtp.User.Id);
        if (resetPasswordToken is null)
            return AuthenticationErrors.ResetPasswordFailed;
        await authenticateRepository.DeleteResetPasswordTokenAsync(storedOtp.User.Id);
        var user = await GetUser(storedOtp.User.UserName!);
        await userManager.ResetPasswordAsync(user.Value, resetPasswordToken.Token, newPassword);
        return true;
    }

    public async Task<Result<VerificationToken>> ResendVerificationCode(
        string otpToken)
    {
        var storedOtp = await authenticateRepository.GetOtpAsync(otpToken);
        if (storedOtp is null)
            return AuthenticationErrors.InvalidOtp;

        var user = storedOtp.User;
        if (await authenticateRepository.IsSentAsync(user.UserName!))
            return AuthenticationErrors.TooManyRequestsForOtp;

        await authenticateRepository.DeleteOtpAsync(otpToken);

        var otp = new Otp(Guid.NewGuid().ToString(), user, storedOtp.IsNew, storedOtp.LoginAs);
        await authenticateRepository.InsertOtpAsync(otp);
        await authenticateRepository.InsertSentAsync(user.UserName!);

        try
        {
            await communicationService.SendVerificationAsync(user.PhoneNumber!, await GenerateOtp(user));
        }
        catch
        {
            return CommunicationErrors.SmsError;
        }

        return new VerificationToken(user.PhoneNumber!, otp.Token);
    }



    #region PrivateMethods
    private async Task updateUserInfoFromYazdSso(ApplicationUser user, MyYazdUser? myYazdUser)
    {
        if (myYazdUser is null)
            return;
        user.NationalId = myYazdUser.NationalId ?? user.NationalId;
        if (myYazdUser.Gender is not null)
        {
            user.Gender = myYazdUser.Gender == 1 ? Gender.Male : Gender.Female;
        }
        if (myYazdUser.Birthday is not null)
        {
            try
            {
                var parts = myYazdUser.Birthday.Split('-');
                user.BirthDate = new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
            }
            catch { }
        }
        user.FirstName = myYazdUser.FirstName ?? user.FirstName;
        user.LastName = myYazdUser.LastName ?? user.LastName;

        if (myYazdUser.Avatar is not null)
        {
            var avatarStreamResult = await myYazdService.GetUserAvatar(myYazdUser.Avatar);
            if (avatarStreamResult.IsSuccess)
            {
                user.Avatar = await storageService.WriteFileAsync(avatarStreamResult.Value, AttachmentType.Avatar, "jpg");
            }
        }

        userRepository.Update(user);
        await unitOfWork.SaveAsync();

        return;
    }

    private async Task<Result> createCitizen(ApplicationUser user)
    {
        try
        {
            user.PhoneNumberConfirmed = true;
            user.SmsAlert = true;
            var result = await userManager.CreateAsync(user, "ADGJL';khfs12");
            if (!result.Succeeded)
                return AuthenticationErrors.UserCreationFailed;

            var result2 = await userManager.AddToRoleAsync(user, RoleNames.Citizen);

            if (!result2.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return AuthenticationErrors.UserCreationFailed;
            }
        }
        catch
        {
            return AuthenticationErrors.UserCreationFailed;
        }
        return Result.Ok();
    }

    private async Task<Result<VerificationToken>> sendVerificationCode(
        ApplicationUser user,
        bool isNew = false,
        ApplicationUser? loginAs = null)
    {
        if (await authenticateRepository.IsSentAsync(user.UserName!))
            return AuthenticationErrors.TooManyRequestsForOtp;

        var otp = new Otp(Guid.NewGuid().ToString(), user, isNew, loginAs);
        await authenticateRepository.InsertOtpAsync(otp);
        await authenticateRepository.InsertSentAsync(user.UserName!);

        if (!IsPhoneNumberValid(user.PhoneNumber))
            return AuthenticationErrors.InvalidPhoneNumber;
        try
        {
            await communicationService.SendVerificationAsync(user.PhoneNumber!, await GenerateOtp(user));
        }
        catch
        {
            return CommunicationErrors.SmsError;
        }

        return new VerificationToken(user.PhoneNumber!, otp.Token);
    }

    private async Task<Result<ApplicationUser>> GetUser(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            return AuthenticationErrors.UserNotFound;
        }

        return user;
    }

    private async Task<string> GenerateOtp(ApplicationUser user)
    {
        //TODO: Use purpose for verification, reset password, etc
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.GenerateAsync("Phone number verification", userManager, user);
    }

    private async Task<bool> ValidateOtp(ApplicationUser user, string token)
    {
        PhoneNumberTokenProvider<ApplicationUser> tokenGenerator = new PhoneNumberTokenProvider<ApplicationUser>();
        return await tokenGenerator.ValidateAsync("Phone number verification", token, userManager, user);
    }

    private async Task<AuthToken> GenerateToken(ApplicationUser user)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(AppClaimTypes.InstanceId, (user.ShahrbinInstanceId ?? -1).ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.Secret));
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: jwtInfo.Issuer,
            audience: jwtInfo.Audience,
            expires: now.Add(jwtInfo.AccessTokenValidDuration),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        var refreshToken = new RefreshToken(
            Guid.NewGuid().ToString(),
            user.Id,
            token.Id,
            now,
            jwtInfo.RefreshTokenValidDuration);
        await authenticateRepository.InsertRefreshTokenAsync(refreshToken);

        return new AuthToken(new JwtSecurityTokenHandler().WriteToken(token), refreshToken.Token);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var noExpiryTokenValidationParameters = tokenValidationParameters.Clone();
            noExpiryTokenValidationParameters.ValidateLifetime = false;
            var principal = tokenHandler.ValidateToken(
                token,
                noExpiryTokenValidationParameters,
                out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
    }

    private bool IsPhoneNumberValid(string? phoneNumber)
    {
        if (phoneNumber is null)
            return false;
        Regex regex = new Regex(@"^09[0-9]{9}$");
        return regex.IsMatch(phoneNumber);
    }
    #endregion
    
}

public record JwtInfo(
    string Secret,
    string Issuer,
    string Audience,
    TimeSpan AccessTokenValidDuration,
    TimeSpan RefreshTokenValidDuration);

public record RefreshToken(
    string Token,
    string UserId,
    string JwtId,
    DateTime CreationDate,
    TimeSpan Expiry);

public record Otp(
    string Token,
    ApplicationUser User,
    bool IsNew,
    ApplicationUser? LoginAs = null);

public record ResetPasswordToken(
    string UserId,
    string Token);