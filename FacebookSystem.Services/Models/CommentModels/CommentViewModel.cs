namespace FacebookSystem.Services.Models.CommentModels
{
    using System;

    using FacebookSystem.Services.Models.LikeModels;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public OwnerViewModel PostOwner { get; set; }

        public OwnerViewModel CommentOwner { get; set; }

        public LikeViewModel Likes { get; set; }
    }
}