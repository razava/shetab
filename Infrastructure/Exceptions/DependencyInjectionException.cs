namespace Infrastructure.Exceptions;

public class NullConnectionStringException : Exception { }  //"Connection string cannot be null."
public class NullJwtSecretException : Exception { }
public class NullJwtValidIssuerException : Exception { }
public class NullJwtValidAudienceException : Exception { } 
public class MessageBrokerConfigurationsNotFoundException : Exception { }  //"Message broker configurations not found"
