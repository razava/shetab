namespace Infrastructure.Exceptions;

public class ResetPasswordTokenGenerationFailedException : Exception
{
    public ResetPasswordTokenGenerationFailedException() : base("ایجاد توکن بازیابی رمز عبور ناموفق بود.") { }
}


public class InvalidVerificationCodeException : Exception
{
    public InvalidVerificationCodeException() : base("کد تایید نامعتبر است.") { }
}



//public class InvalidLoginException : Exception { }
//public class InvalidUsernameException : Exception { }
//public class UserAlreadyExsistsException : Exception { }
//public class UserRegisterException : Exception { }
//public class UserNotExistException : Exception { }




