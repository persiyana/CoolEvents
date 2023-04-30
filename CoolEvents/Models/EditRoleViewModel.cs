using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoolEvents.Models
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string? Id { get; set; }
        [Display(Name = "Role:")]
        public string? RoleName { get; set; }
        public SelectList? Roles { get; set; }
        [Display(Name = "Username:")]
        public string? Username { get; set; }
        [Display(Name = "Email:")]
        public string Email { get; set; }
        public List<string>? Users { get; set; }

    }
}
