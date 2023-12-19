namespace Application.Common.Exceptions;

public class InvalidCityException : Exception
{
    public InvalidCityException(string message) : base(message)
    {
        
    }
}

public class DuplicateInstanceException : Exception
{
    public DuplicateInstanceException(string message) : base(message)
    {

    }
}
 
public class ForbidNullParentCategoryException : Exception { }   //"Parent category cannot be null."
public class ForbidNullProcessException : Exception { }   //"Process cannot be null here."
public class ForbidNullRolesException : Exception { }   //"Roles cannot be null here."


