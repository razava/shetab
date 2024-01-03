namespace Infrastructure.Exceptions;

public class NullConnectionStringException : Exception
{
    public NullConnectionStringException() : base("Connection string cannot be null.") { }
} 


public class NullJwtSecretException : Exception
{
    public NullJwtSecretException() : base("JwtSecret is null") { }
}


public class NullJwtValidIssuerException : Exception
{
    public NullJwtValidIssuerException() : base("JwtValidIssuer is null") { }
}


public class NullJwtValidAudienceException : Exception
{
    public NullJwtValidAudienceException() : base("JwtValidAudience is null.") { }
}


public class MessageBrokerConfigurationsNotFoundException : Exception
{
    public MessageBrokerConfigurationsNotFoundException() : base("Message broker configurations not found.") { }
} 
