﻿using Application.Common.Interfaces.Security;
using SixLaborsCaptcha.Core;
using SixLabors.ImageSharp;

namespace Infrastructure.Captcha;

public class SixLaborsCaptchaProvider : ICaptchaProvider
{
    private SixLaborsCaptchaModule captchaRandomImage;
    //TODO: Use Redis or some thing else
    private static HashSet<Tuple<Guid, string, DateTime>> keyValuePairs = new();

    public SixLaborsCaptchaProvider()
    {
        captchaRandomImage = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
        {
            DrawLines = 7,
            TextColor = new Color[] { Color.Blue, Color.Black },
            FontFamilies = new string[] { "Marlboro" }
        });
    }

    public CaptchaResultModel GenerateImage(string text = "")
    {
        Random rnd = new Random();
        if (text == "")
            text = rnd.Next(10000, 99999).ToString();

        var result = captchaRandomImage.Generate(text);

        var key = Guid.NewGuid();
        keyValuePairs.Add(new Tuple<Guid, string, DateTime>(key, text.ToUpper(), DateTime.Now.AddMinutes(15)));

        return new CaptchaResultModel(key, result);
    }

    public string GetRandomString(int length)
    {
        return "";
    }

    public bool Validate(CaptchaValidateModel model)
    {
        var t = keyValuePairs.FirstOrDefault(p => p.Item1 == model.Key);
        if (t == null)
            return false;

        keyValuePairs.Remove(t);

        if (t.Item3 < DateTime.Now)
            return false;
        if (t.Item2 != model.Value.ToUpper())
            return false;

        return true;
    }
}
