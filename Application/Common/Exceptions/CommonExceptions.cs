namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string objectName) : base($"{objectName} Not Found!")
    {
        
    }
}

public class CreationFailedException : Exception
{
    public CreationFailedException(string objectName) : base($"{objectName} Not Found!")
    {

    }
}
 
public class CommentHasReplyException : Exception { }  //"This comment has a reply."
public class SaveImageFailedException : Exception { }  
public class LoopMadeException : Exception      //"حلقه ای در ساختار سازمانی ایجاد شده است."
{
    public LoopMadeException(string message) : base(message)
    {
        
    }
}

public class AccessDeniedException : Exception      
{
    public AccessDeniedException() : base() { }
    public AccessDeniedException(string message) : base(message)
    {

    }
}


public class AttachmentsFailureException() : Exception { }
public class ExecutiveOnlyLimitException() : Exception { } //"User must be an executive to be able to send message to citizen."

public class NullActorRolesException : Exception { }
 



