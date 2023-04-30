using Microsoft.AspNetCore.Identity;
using System;
using System.Data;

namespace CoolEvents.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserEvent>? UserEvents { get; set; }
    }
}
