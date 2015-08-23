namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

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

        public string OwnerId { get; set; }

        public int? GroupId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

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
