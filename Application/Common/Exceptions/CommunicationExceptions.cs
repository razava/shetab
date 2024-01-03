namespace Application.Common.Exceptions;

public class SendSmsException : Exception
{
    public SendSmsException() : base("خطایی در ارسال پیام رخ داد.") { }
}




