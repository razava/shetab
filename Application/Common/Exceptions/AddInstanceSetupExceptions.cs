namespace Application.Common.Exceptions;

public class InvalidCityException : Exception
{
    public InvalidCityException(string message) : base(message) { }
}


public class DuplicateInstanceException : Exception
{
    public DuplicateInstanceException(string message) : base(message) { }
}

 
public class ForbidNullParentCategoryException : Exception
{
    public ForbidNullParentCategoryException() : base("پدر دسته بندی نباید خالی باشد.") { }
}   


public class ForbidNullRolesException : Exception
{
    public ForbidNullRolesException() : base("نقش ها نباید خالی باشد.") { }
} 


