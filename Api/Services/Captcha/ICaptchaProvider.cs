namespace Api.Services;

public interface ICaptchaProvider
{
    public string GetRandomString(int length);
    public byte[] GenerateImage(out Guid key, string text = "");
    public bool Validate(Guid key, string value);
}
