
namespace Domain.Exceptions;

public class ParticipationMoreThanOnceException : Exception
{
    public ParticipationMoreThanOnceException() : base("کاربر قبلا در این نظرسنجی شرکت کرده است.") { }
}    //"User has participated in this poll before."


public class NullAnswerTextException : Exception
{
    public NullAnswerTextException() : base("متن در نظرسنجی تشریحی نمی تواند خالی باشد.") { }
}   //"Text cannot be null in descriptive polls."


public class OneChoiceAnswerLimitException : Exception
{
    public OneChoiceAnswerLimitException() : base("نظرسنجی تک انتخابی باید فقط یک پاسخ داشته باشد.") { }
}  //"Single choice polls must have exactly 1 answer."


public class InvalidChoiceException : Exception
{
    public InvalidChoiceException() : base("انتخاب نامعتبر است.") { }
}  

