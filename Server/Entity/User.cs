using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string Name { get; set; }

    public ICollection<Store> Stores { get; set; }
}
