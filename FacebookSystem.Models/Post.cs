namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;

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

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser Owner { get; set; }

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
