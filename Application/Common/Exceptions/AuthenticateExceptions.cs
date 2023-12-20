namespace Application.Common.Exceptions;
public class PhoneNumberNotConfirmedException : Exception
{
    public PhoneNumberNotConfirmedException(string? message) : base(message)
    {

    }
}
public class InvalidCaptchaException : Exception { }
public class RegisterCitizenException :Exception { }



public class UserCreationFailedException :Exception { }
public class NullAssignedRoleException :Exception { }












//.............................................................
public class InvalidUsernameException : Exception { }
public class UserAlreadyExsistsException : Exception { }
public class UserRegisterException : Exception { }
public class UserNotExistException : Exception { }
public class ResetPasswordTokenGenerationFailed : Exception { }
public class InvalidVerificationCode : Exception { }
public class InvalidLoginException : Exception { }
