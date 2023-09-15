namespace Api.Configurations;

public class AppVersions
{
    public const string Name = "AppVersions";

    public List<AppVersion>? AppVersionList { get; set; } = new List<AppVersion>();
}

public class AppVersion
{
    public string Type { get; set; } = null!;
    public string VersionName { get; set; } = null!;
    public int VersionCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsCritical { get; set; }
    public string Url1 { get; set; } = string.Empty;
    public string Url2 { get; set; } = string.Empty;
    public string Url3 { get; set; } = string.Empty;
    public string Url4 { get; set; } = string.Empty;
    public string Url5 { get; set; } = string.Empty;
}