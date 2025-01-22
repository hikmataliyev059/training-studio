using Microsoft.AspNetCore.Identity;

namespace training_studio.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
}
