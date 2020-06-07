using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Auth.Application.Model
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Policy> Policies { get; set; }
    }
}
