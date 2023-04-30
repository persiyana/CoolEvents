using System.ComponentModel.DataAnnotations;

namespace CoolEvents.Models
{
    public class DetailsEventModel
    {
        public int Id { get; set; }
        [Display(Name = "Name:")]
        public string Name { get; set; }
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Display(Name = "Image:")]
        public string? FilePath { get; set; }
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
        [Display(Name = "Going:")]
        public List<ApplicationUser> ApplicationUsers { get; set; }

    }
}
