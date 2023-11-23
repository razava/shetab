﻿using Api.Services.Authentication;
using System.Security.Claims;

namespace Api.ExtensionMethods;

public static class ClaimsPrincipalExtensionMethods
{
    public static string GetUserName(this ClaimsPrincipal value)
    {
        var result = value.FindFirstValue(ClaimTypes.Name);
        if (result is null)
        {
            throw new Exception(); 
        }
        return result;
    }

    public static string GetUserId(this ClaimsPrincipal value)
    {
        var result = value.FindFirstValue(ClaimTypes.NameIdentifier);
        if (result is null)
        {
            throw new Exception();
        }
        return result;
    }

    public static List<string> GetUserRoles(this ClaimsPrincipal value)
    {
        return value.FindAll(ClaimTypes.Role).Select(p => p.Value).ToList();
    }

    public static int GetUserInstanceId(this ClaimsPrincipal value)
    {
        int result;
        if(!int.TryParse(value.FindFirstValue(AppClaimTypes.InstanceId), out result))
        {
            throw new Exception();
        }

        return result;
    }
}
