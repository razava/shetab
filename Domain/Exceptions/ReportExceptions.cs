namespace Domain.Exceptions;

public class CategoryHaveNoAssignedProcess : Exception { }  //"Category should have a process assigned to."
public class NotLoadedProcessException : Exception { }  //"Process is not loaded."
public class NullCurrentStageException : Exception { }  //
public class NullStageException : Exception { }  //
public class BotNotFoundException : Exception { }  //"Bot not found." 
public class ForbidNullTransitionException : Exception { }  //"Transition cannot be null here."




