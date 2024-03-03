namespace SharedKernel.Errors;

//todo : set statuscode with metadata for errors.

public static class NotFoundErrors
{
    public static readonly Error User = new Error("کاربر یافت نشد.");
    public static readonly Error Report = new Error("گزارش یافت نشد.");
    public static readonly Error Category = new Error("دسته بندی یافت نشد.");
    public static readonly Error OrganizationalUnit = new Error("واحد سازمانی یافت نشد.");
    public static readonly Error Executive = new Error("واحد اجرایی یافت نشد.");
    public static readonly Error Comment = new Error("نظر یافت نشد.");
    public static readonly Error Feedback = new Error("بازخورد یافت نشد.");
    public static readonly Error FAQ = new Error("سوال متداول یافت نشد.");
    public static readonly Error News = new Error("خبر یافت نشد.");
    public static readonly Error Poll = new Error("نظر سنجی یافت نشد.");
    public static readonly Error Process = new Error("فرایند یافت نشد.");
    public static readonly Error QuickAccess = new Error("دسترسی سریع یافت نشد.");
    public static readonly Error Form = new Error("فرم یافت نشد.");
    public static readonly Error ReportNote = new Error("یادداشت یافت نشد.");
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
    public static readonly Error ChangePasswordFailed = new Error("خطای تغییر رمز");
    public static readonly Error RoleAssignmentFailed = new Error("تخصیص نقش ناموفق بود.");
    public static readonly Error VerificationFailed = new Error("اعتبار سنجی ناموفق.");

}

public static class OperationErrors
{
    public static readonly Error General = new Error("خطایی رخ داد.");
    public static readonly Error CommentHasReply = new Error("این کامنت یک ریپلای دارد.");
    public static readonly Error LoopMade = new Error("حلقه ای در ساختار سازمانی ایجاد شده است.");
    public static readonly Error ExecutiveOnlyLimit = new Error("کاربر برای ارسال پیام به شهروند باید نقش واحد اجرایی داشته باشد.");
    public static readonly Error FormIsInUse = new Error("امکان حذف فرم های در حال استفاده وجود ندارد..");
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
    public static readonly Error Actor = new Error("خطایی رخ داد.");
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
    public static readonly Error Process = new Error("ایجاد فرایند ناموفق بود.");
}

public static class AccessDeniedErrors
{
    public static readonly Error General = new Error("محدودیت دسترسی.");
    public static readonly Error Executive = new Error("فقط واحد اجرایی می تواند پیمانکار تعریف کند.");
    public static readonly Error Actor = new Error("کاربر جاری Actor نیست."); // not used.
}

public static class MyYazdErrors
{
    public static readonly Error General = new Error("ارتباط با یزد من ناموفق بود.");
}