using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;

    public ICollection<Store> Stores { get; set; } = new List<Store>();
}
