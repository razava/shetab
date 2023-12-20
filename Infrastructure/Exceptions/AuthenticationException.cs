namespace Infrastructure.Exceptions;

public class PhoneNumberNotConfirmedException : Exception
{
    public PhoneNumberNotConfirmedException(string? message) : base(message)
    {

    }
}

public class InvalidLoginException : Exception { }
public class InvalidUsernameException : Exception { }
public class UserAlreadyExsistsException : Exception { }
public class UserRegisterException : Exception { }
public class UserNotExistException : Exception { }
public class ResetPasswordTokenGenerationFailedException : Exception { }
public class InvalidVerificationCodeException : Exception { } 




