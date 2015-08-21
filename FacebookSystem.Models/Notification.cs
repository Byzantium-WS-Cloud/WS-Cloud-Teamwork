namespace FacebookSystem.Models
{
    using System;

    using Enums;

    public class Notification
    {
        public int Id { get; set; }

        public NotificationType Notification { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Seen { get; set; }

        public ApplicationUser ByUser { get; set; }
 
        public ApplicationUser Owner { get; set; }
    }
}
