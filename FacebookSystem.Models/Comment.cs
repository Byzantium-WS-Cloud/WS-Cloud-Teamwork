namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Comment
    {
        private ICollection<CommentLike> likes;

        public Comment()
        {
            this.likes = new HashSet<CommentLike>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser PostOwner { get; set; }

        public ApplicationUser CommentOwner { get; set; }

        public ICollection<CommentLike> Likes 
        {
            get { return this.likes; }
            set { this.likes = value; }
        }
    }
}
