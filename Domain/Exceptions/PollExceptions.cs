
namespace Domain.Exceptions;

public class ParticipationMoreThanOnceException : Exception { }    //"User has participated in this poll before."
public class NullAnswerTextException : Exception { }   //"Text cannot be null in descriptive polls."
public class OneChoiceAnswerLimitException : Exception { }  //"Single choice polls must have exactly 1 answer."
public class InvalidChoiceException : Exception { }  //

