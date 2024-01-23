using Microsoft.AspNetCore.Identity;

namespace Application.Common.Exceptions;

public class PhoneNumberNotConfirmedException : Exception
{
    public PhoneNumberNotConfirmedException() : base("ابتدا شماره تلفن را اعتبار سنجی کنید.") { }
}

public class InvalidCaptchaException : Exception
{
    public InvalidCaptchaException() : base("کپچا اشتباه است.") { }
}


//public class UserCreationFailedException :Exception { }
public class NullAssignedRoleException :Exception
{
    public NullAssignedRoleException() : base("نقشی برای تخصیص انتخاب نشده.") { }
}


public class InvalidUsernameException : Exception
{
    public InvalidUsernameException() : base("نام کاربری نامعتبر است.") { }
}

public class UserAlreadyExsistsException : Exception
{
    public UserAlreadyExsistsException() : base("کاربری با این مشخصات وجود دارد.") { }
}


public class InvalidLoginException : Exception 
{
    public InvalidLoginException() : base("ورود نامعتبر") { }
}
 
public class UserCreationFailedException : Exception
{
    public UserCreationFailedException(List<IdentityError> errors) : base("خطای سرور.")
    {
        Data.Add("Errors", errors);
    }
}



//public class ResetPasswordTokenGenerationFailed : Exception { }
//public class InvalidVerificationCode : Exception { }