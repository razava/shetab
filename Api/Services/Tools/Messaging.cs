namespace Api.Services.Tools;

public static class Messaging
{
    public static string VerificationCode(string code)
    {
        var message = "سامانه شهربین\r\nکدورود:\r\n" + code;
        return message;
    }

    public static string ReportCreated(string username, string trackingNumber, string password)
    {
        var message = $"درخواست شما با کد رهگیری {trackingNumber} در سامانه شهربین ثبت شد.";
        if (password != "")
        {
            message += "\r\n";
            message += "لطفاً برای پیگیری برنامه شهربین را نصب کرده و برای ورود از داده های زیر استفاده نمایید:\r\n";
            message += $"نام کاربری:{username}\r\nرمزعبور:{password}";
        }
        return message;
    }

    public static string ReportFinished(string trackingNumber)
    {
        var message = $"درخواست شما با کد رهگیری {trackingNumber} رسیدگی شد. به منظور مشاهده نتیجه به وبسایت یا برنامه کاربردی شهربین مراجعه نمایید.";
        return message;
    }

    public static string ContractorCreated(string username, string password)
    {
        string message;
        message = "پیمانکار گرامی، عضویت شما در سامانه شهربین با موفقیت انجام شد.";
        message += "\r\n";
        message += "لطفاً برای ورود از داده های زیر استفاده نمایید:\r\n";
        message += $"نام کاربری:{"c-" + username}\r\nرمزعبور:{password}";

        return message;
    }

    public static string FeedbackRequest(string url)
    {
        var message = "درخواست شما در سامانه شهربین رسیدگی شد. لطفاً میزان رضایتمندی خود را از طریق لینک زیر با ما در میان بگذارید.";
        if (url != "")
        {
            message += "\r\n";
            message += $"{url}";
        }
        return message;
    }


}