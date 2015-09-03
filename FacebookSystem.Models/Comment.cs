namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        private ICollection<CommentLike> likes;

        public Comment()
        {
            this.likes = new HashSet<CommentLike>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]

        public string Content { get; set; }

        public int PostId { get; set; }


        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser PostOwner { get; set; }

        public virtual ApplicationUser CommentOwner { get; set; }

        public bool IsCommentHidden { get; set; }

        public virtual ICollection<CommentLike> Likes 
        {
            get { return this.likes; }
            set { this.likes = value; }
        }
    }
}
