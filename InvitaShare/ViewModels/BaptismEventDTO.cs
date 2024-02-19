namespace InvitaShare.ViewModels
{
    public class BaptismEventDTO
    {
        public int Id { get; set; }
        public string? ChildName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherName { get; set; }
        public string? GodParent1 { get; set; }
        public string? GodParent2 { get; set; }

        public string EventName { get; set; } = null!;
        public string EventType { get; set; } = "Baptism";
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
