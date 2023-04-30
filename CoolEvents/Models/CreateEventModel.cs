using System.ComponentModel.DataAnnotations;

namespace CoolEvents.Models
{
    public class CreateEventModel
    {
        public int Id { get; set; }
        [Display(Name = "Name:")]
        public string Name { get; set; }
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Display(Name = "Image:")]
        public IFormFile? Image { get; set; }
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
    }
}
