namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        public int PostId { get; set; }

        public string PostOwnerId { get; set; }

        public string CommentOwnerId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser PostOwner { get; set; }

        public virtual ApplicationUser CommentOwner { get; set; }

        public virtual ICollection<CommentLike> Likes 
        {
            get { return this.likes; }
            set { this.likes = value; }
        }
    }
}
