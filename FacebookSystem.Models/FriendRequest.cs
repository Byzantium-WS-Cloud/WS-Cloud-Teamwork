namespace FacebookSystem.Models
{
    using Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FromId { get; set; }

        public virtual ApplicationUser From { get; set; }

        [Required]
        public string ToId { get; set; }

        public virtual ApplicationUser To { get; set; }

        public FriendRequestStatus Status { get; set; }
    }
}
