namespace CoolEvents.Models
{
    public class AddNewEventModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public ApplicationUser? User { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public Event? Event { get; set; }
    }
}
