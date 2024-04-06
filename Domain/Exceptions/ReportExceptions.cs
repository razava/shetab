namespace Domain.Exceptions;

public class CategoryHaveNoAssignedProcess : Exception
{
    public CategoryHaveNoAssignedProcess() : base(" امکان ثبت درخواست با این دسته بندی وجود ندارد.")
    {
        
    }
}  //"Category should have a process assigned to."


public class NotLoadedProcessException : Exception
{
    public NotLoadedProcessException() : base("خطایی رخ داد.")
    {
        
    }
}  //"Process is not loaded."


public class NullCurrentStageException : Exception
{
    public NullCurrentStageException() : base("خطایی رخ داد.")
    {
        
    }
}  //


public class NullStageException : Exception
{
    public NullStageException() : base("خطایی رخ داد.")
    {
        
    }
}  //


public class BotNotFoundException : Exception
{
    public BotNotFoundException() : base("خطایی رخ داد.")
    {
        
    }
}  //"Bot not found." 


public class ForbidNullTransitionException : Exception
{
    public ForbidNullTransitionException() : base("خطایی رخ داد.") { }
}  //"Transition cannot be null here."






