using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Relational.IdentityAggregate;

public class ApplicationRole : IdentityRole
{
    public string Title { get; set; } = string.Empty;
    public List<Chart> Charts { get; set; } = new List<Chart>();
    public List<Category> Categories { get; set; } = new List<Category>();

    //public ApplicationRole(string roleName, string title) : base(roleName)
    //{
    //    Title = title;
    //}
}


