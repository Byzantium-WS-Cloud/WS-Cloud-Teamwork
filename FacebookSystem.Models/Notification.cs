namespace FacebookSystem.Models
{
    using System;

    using Enums;

    public class Notification
    {
        public int Id { get; set; }

        public NotificationType NotificationType { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Seen { get; set; }

        public string ByUserId { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser ByUser { get; set; }
 
        public virtual ApplicationUser Owner { get; set; }
    }
}
