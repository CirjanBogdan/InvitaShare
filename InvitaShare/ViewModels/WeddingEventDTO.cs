namespace InvitaShare.ViewModels
{
    public class WeddingEventDTO
    {
        public int Id { get; set; }
        public string? BrideName { get; set; }
        public string? GroomName { get; set; }
        public string? GodParent1 { get; set; }
        public string? GodParent2 { get; set; }
        public string EventName { get; set; } = null!;
        public string EventType { get; set; } = "Wedding";
        public string? RestaurantName { get; set; }
        public string? ChurchName { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedOnView
        {
            get { return CreatedOn.ToShortDateString(); }
        }
    }
}
