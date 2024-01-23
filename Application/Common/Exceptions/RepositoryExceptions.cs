using Microsoft.AspNetCore.Identity;

namespace Application.Common.Exceptions;


public class ServerNotFoundException : Exception
{
    public ServerNotFoundException(string message, Exception innerEx) : base(message, innerEx)
    {
        
    }
}


//Generic Repository
public class MethodNotFoundException : Exception { }
public class PropertyInfoNotFoundException : Exception { }



//Actor Repository
public class InstanceNotFoundException : Exception { }
public class ActorNotFoundException : Exception { }


//Process Repository
public class ForbidNullRoleException : Exception
{
    public ForbidNullRoleException() : base("خطایی رخ داد.") { }
}   


public class NullActorException : Exception
{
    public NullActorException() : base("خطایی رخ داد.") { }
}


public class ForbidNullProcessException : Exception
{
    public ForbidNullProcessException() : base("فرایند نباید خالی باشد.")
    {
        
    }
}   //"Process cannot be null here."


public class ForbidNullStageException : Exception
{
    public ForbidNullStageException() : base("خطایی رخ داد.")
    {
        
    }
}   //"Stage cannot be null here."


//User Repository
public class RoleAssignmentFailedException : Exception
{
    public RoleAssignmentFailedException(List<IdentityError>? errors) : base("خطای سرور.")
    {
        if(errors != null)
            Data.Add("Errors", errors);
    }
}


public class NullUserRolesException : Exception
{
    public NullUserRolesException() : base("نقش های کاربر خالی است.")
    {
        
    }
}

//public class UserCreationFailedException : Exception { }
//public class UserNotFoundException : Exception { }










