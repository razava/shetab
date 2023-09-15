using SixLaborsCaptcha.Core;

namespace Api.Services.Captcha;

public class CaptchaProvider : ICaptchaProvider
{
    private SixLaborsCaptchaModule captchaRandomImage;
    private HashSet<Tuple<Guid, string, DateTime>> keyValuePairs;

    public CaptchaProvider(IWebHostEnvironment webHostEnvironment)
    {
        captchaRandomImage = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
        {
            DrawLines = 7,
            TextColor = new Color[] { Color.Blue, Color.Black },
            FontFamilies = new string[] { "Marlboro" }
        });
        keyValuePairs = new HashSet<Tuple<Guid, string, DateTime>>();
    }

    public byte[] GenerateImage(out Guid key, string text = "")
    {
        Random rnd = new Random();
        if (text == "")
            text = rnd.Next(10000, 99999).ToString();

        var result = captchaRandomImage.Generate(text);

        key = Guid.NewGuid();
        keyValuePairs.Add(new Tuple<Guid, string, DateTime>(key, text.ToUpper(), DateTime.Now.AddMinutes(15)));

        return result;
    }

    public string GetRandomString(int length)
    {
        return "";
    }

    public bool Validate(Guid key, string value)
    {
        var t = keyValuePairs.FirstOrDefault(p => p.Item1 == key);
        if (t == null)
            return false;

        keyValuePairs.Remove(t);

        if (t.Item3 < DateTime.Now)
            return false;
        if (t.Item2 != value.ToUpper())
            return false;

        return true;
    }
}
