using Application.Common.Interfaces.MyYazd;
using Domain.Models.MyYazd;
using FluentResults;
using Microsoft.Extensions.Logging;
using SharedKernel.Errors;
using System.Text.Json;

namespace Infrastructure.Authentication;

public class MyYazdService(
    HttpClient httpClient,
    ILogger<MyYazdService> logger) : IDisposable, IMyYazdService
{
    public async Task<Result<MyYazdUserInfo>> GetUserInfo(string code)
    {
        try
        {
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "jwjOjhnZzct0AzNSacV0XYOvxOKXrLfnumM8hYPN"),
                new KeyValuePair<string, string>("client_secret", "ahVWYQ4BZnBuvgPHwBFLV4EQvEG6CbOeLTemp8yk4Mqj5Wu2uSeh99f6f1ur0mtAdiOV0Q3iRk7jloYgNk5f6tA5kZnBsuc859kPkoDOCNVe8v1SRhNXTNENFPDIvuGk"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            };
            var url = @"/api/oauth/token/";
            using var req = new HttpRequestMessage(HttpMethod.Post, url) 
            { 
                Content = new FormUrlEncodedContent(nvc),
            };

            using var httpResponse = await httpClient.SendAsync(req);

            httpResponse.EnsureSuccessStatusCode();

            var userInfoJson = await httpResponse.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<MyYazdUserInfo>(userInfoJson);
            if(userInfo is null)
            {
                return MyYazdErrors.General;
            }
            return userInfo;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return MyYazdErrors.General;
        }
    }

    public async Task<Result<MemoryStream>> GetUserAvatar(string url)
    {
        try
        {
            using var httpResponse = await httpClient.GetAsync(url);

            httpResponse.EnsureSuccessStatusCode();

            var result = new MemoryStream();
            await httpResponse.Content.CopyToAsync(result);
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return MyYazdErrors.General;
        }
    }

    public void Dispose() => httpClient?.Dispose();
}

