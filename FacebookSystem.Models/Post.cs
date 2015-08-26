namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        private ICollection<PostLike> likes;
        private ICollection<Comment> comments;

        public Post()
        {
            this.likes = new HashSet<PostLike>();
            this.comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public bool IsPostHidden { get; set; }

        public virtual ICollection<PostLike> Likes 
        {
            get { return this.likes; }
            set { this.likes = value; }
        }

        public virtual ICollection<Comment> Comments 
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}
