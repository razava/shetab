namespace SharedKernel.Successes;

public static class CreationSuccess
{
    public static readonly Success Report = new Success("درخواست با موفقیت ایجاد شد.");
    public static readonly Success User = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success Category = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success OrganizationalUnit = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success Process = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success Poll = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success News = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success Faq = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success QuickAccess = new Success("کاربر با موفقیت ایجاد شد.");
    public static readonly Success Form = new Success("کاربر با موفقیت ایجاد شد.");

}


public static class Registration
{
    public static readonly Success PollResponse = new Success("نظر شما با موفقیت ثبت شد.");
    public static readonly Success ReportNote = new Success("یادداشت شما با موفقیت ثبت شد.");
}


public class SuccessOperation
{
    public static readonly Success MessageToCitizen = new Success("پیام به شهروند با موفقیت ارسال شد");
}