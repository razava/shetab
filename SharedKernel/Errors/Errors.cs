namespace SharedKernel.Errors;

//todo : set statuscode with metadata for errors.

public static class NotFoundErrors
{
    public static readonly Error User = new Error("");
    public static readonly Error Report = new Error("");
    public static readonly Error Category = new Error("دسته بندی یافت نشد.");
    public static readonly Error OrganizationalUnit = new Error("");
    public static readonly Error Executive = new Error("");
    public static readonly Error Comment = new Error("");
    public static readonly Error Feedback = new Error("");
    public static readonly Error FAQ = new Error("");
    public static readonly Error News = new Error("");
    public static readonly Error Poll = new Error("");
    public static readonly Error Process = new Error("");
    public static readonly Error QuickAccess = new Error("");
    
}


public static class AuthenticateErrors
{
    public static readonly Error InvalidCaptcha = new Error("کپچا اشتباه است.");
    public static readonly Error PhoneNumberNotConfirmed = new Error("ابتدا شماره تلفن را اعتبار سنجی کنید.");
    public static readonly Error SendSms = new Error("خطایی در ارسال پیام رخ داد.");
    public static readonly Error UserCreationFailed = new Error("");
    public static readonly Error NullAssignedRole = new Error("نقشی برای تخصیص انتخاب نشده.");
    public static readonly Error InvalidUsername = new Error("نام کاربری نامعتبر است.");
    public static readonly Error UserAlreadyExsists = new Error("کاربری با این مشخصات وجود دارد.");
    public static readonly Error InvalidLogin = new Error("ورود نامعتبر");

}

public static class CommenErrors
{
    public static readonly Error CommentHasReply = new Error("این کامنت یک ریپلای دارد.");
    public static readonly Error LoopMade = new Error("حلقه ای در ساختار سازمانی ایجاد شده است.");
    public static readonly Error ExecutiveOnlyLimit = new Error("کاربر برای ارسال پیام به شهروند باید نقش واحد اجرایی داشته باشد.");

}

public static class AttachmentErrors
{
    public static readonly Error SaveImageFailed = new Error("ذخیره عکس ناموفق بود.");
    public static readonly Error AttachmentsFailure = new Error("خطای پیوست فایل!"); 
}

public static class ServerNotFoundErrors
{
    //public static readonly Error OrganizationalUnit = new Error("");
    public static readonly Error ExecutiveUser = new Error("خطایی رخ داد.");
    public static readonly Error Feedback = new Error("خطایی رخ داد.");
}

public static class ServerNullErrors
{
    public static readonly Error NullActorRole = new Error("");
    public static readonly Error ForbidNullRoles = new Error("خطایی رخ داد.");
    public static readonly Error NullActor = new Error("خطایی رخ داد.");
}

public static class AddInstanceErrors
{
    public static readonly Error InvalidCity = new Error("");
    public static readonly Error DuplicateInstance = new Error("");
    public static readonly Error ForbidNullParentCategory = new Error("پدر دسته بندی نباید خالی باشد.");
    public static readonly Error ForbidNullRoles = new Error("نقش ها نباید خالی باشد.");
    public static readonly Error ForbidNullProcess = new Error("فرایند نباید خالی باشد.");
}

public static class RepositoryErrors
{
    

}

public static class BadRequestErrors
{

}

public static class CreationFailedErrors
{

}

public static class AccessDeniedErrors
{
    
}

public static class CommunicationErrors
{
    public static readonly Error SendSms = new Error("خطایی در ارسال پیام رخ داد.");
}


