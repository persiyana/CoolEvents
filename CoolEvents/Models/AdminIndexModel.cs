using Microsoft.AspNetCore.Identity;

namespace CoolEvents.Models
{
    public class AdminIndexModel
    {
        public List<ApplicationUser> Users { get; set; }

        public Dictionary<ApplicationUser, string> UserRoles { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminIndexModel(UserManager<ApplicationUser> usermanager)
        {
            _userManager = usermanager;

        }
        public async Task AddToList()
        {
            foreach (var u in Users)
            {
                string s = _userManager.GetRolesAsync(u).Result[0];
                UserRoles.Add(u, s);
            }

        }
    }
}
