using System.ComponentModel.DataAnnotations;

namespace CoolEvents.Models
{
    public class Event
    {
        [Key] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name:")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Image is required.")]
        [Display(Name = "Image:")]
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
        public virtual ICollection<UserEvent>? UserEvents { get; set; }
    }
}
