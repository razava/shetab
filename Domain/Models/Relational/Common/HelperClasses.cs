namespace Domain.Models.Relational.Common;

public class InputForExecutive
{
    public string Code { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsPerRegion { get; set; }
    public List<Region> Regions { get; set; } = new List<Region>();
}
