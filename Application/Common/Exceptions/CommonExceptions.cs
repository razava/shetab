namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string objectName) : base($"{objectName} یافت نشد.") { }
}


public class CreationFailedException : Exception
{
    public CreationFailedException(string objectName) : base($"ایجاد {objectName} ناموفق بود.") { }
}


public class AccessDeniedException : Exception
{
    public AccessDeniedException() : base() { }
    public AccessDeniedException(string message) : base(message) { }
}


public class FeedbackNotFoundException : Exception { }
public class ExecutiveUserNotFoundException : Exception { }
public class NullActorRolesException : Exception { }


public class CommentHasReplyException : Exception
{
    public CommentHasReplyException() : base("این کامنت یک ریپلای دارد.") { }
} 


public class SaveImageFailedException : Exception
{
    public SaveImageFailedException() : base("ذخیره عکس ناموفق بود.") { }
}  


public class LoopMadeException : Exception    
{
    public LoopMadeException() : base("حلقه ای در ساختار سازمانی ایجاد شده است.") { }
}


public class AttachmentsFailureException : Exception
{
    public AttachmentsFailureException() : base("خطای پیوست فایل!") { }
}


public class ExecutiveOnlyLimitException : Exception
{
    public ExecutiveOnlyLimitException() : base("کاربر برای ارسال پیام به شهروند باید نقش واحد اجرایی داشته باشد.") { }
} 

 



