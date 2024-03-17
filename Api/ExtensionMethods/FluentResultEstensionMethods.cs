using Microsoft.AspNetCore.Mvc;

namespace Api.ExtensionMethods;

public static class FluentResultEstensionMethods
{
    public static ActionResult Match<TValue>(
        this Result<TValue> result,
        Func<Result<TValue>, ActionResult> success,
        Func<Result, ActionResult> failure)
    {
        if (result.IsSuccess)
            return success(result);
        else
            return failure(result.ToResult());
    }

    //public static ActionResult Match2<TValue>(
    // this Result<TValue> result,
    // Func<TValue, ActionResult> success,
    // Func<Result, ActionResult> failure)
    //{
    //    if (result.IsSuccess)
    //        return success(result.Value);
    //    else
    //        return failure(result.ToResult());
    //}
}
