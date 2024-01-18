namespace InvitaShare.Models
{
    public class BaptismEvent : Event
    {
        public string ChildName { get; set; }
        public string ParentName { get; set; }
        public string? ChurchName { get; set; }
        public string? RestaurantName { get; set; }
    }
}
