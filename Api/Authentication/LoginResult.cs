namespace Api.Authentication;

public class LoginResult
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public IList<string> Roles { get; set; }
    public DateTime CurrentTime { get; set; }
}
