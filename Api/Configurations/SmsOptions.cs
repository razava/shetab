namespace Api.Configurations;

public class SmsOptions
{
    public const string Name = "SmsOptions";

    //KavehNegar
    public string ApiKey { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string TemplateName { get; set; } = null!;

    //IpPanel
    public string IpPanelAccessKey { get; set; } = null!;
    public string IpPanelPatternId { get; set; } = null!;
    public string IpPanelPhoneNumber { get; set; } = null!;

    //Magfa
    public string MagfaPhoneNumber { get; set; } = null!;
    public string MagfaUsername { get; set; } = null!;
    public string MagfaDomain { get; set; } = null!;
    public string MagfaPassword { get; set; } = null!;
    public string MagfaUrl { get; set; } = null!;

    //Sms Negar
    public string SmsNegarUrl { get; set; } = null!;
    public string SmsNegarUsername { get; set; } = null!;
    public string SmsNegarPassword { get; set; } = null!;
    public string SmsNegarDomain { get; set; } = null!;
    public string SmsNegarPhoneNumber { get; set; } = null!;

    //Magfa Proxy
    public string MagfaProxyUrl { get; set; } = null!;

    //Firebase Proxy
    public string FirebaseProxyUrl { get; set; } = null!;

    //1b1
    public string YekBYekUsername { get; set; } = null!;
    public string YekBYekPassword { get; set; } = null!;
    public string YekBYekDomain { get; set; } = null!;
    public string YekBYekPhoneNumber { get; set; } = null!;

    //MeliPayamak
    public string MeliPayamakUsername { get; set; } = null!;
    public string MeliPayamakPassword { get; set; } = null!;
    public string MeliPayamakPhoneNumber { get; set; } = null!;
    public string MeliPayamakPatternId { get; set; } = null!;

}
