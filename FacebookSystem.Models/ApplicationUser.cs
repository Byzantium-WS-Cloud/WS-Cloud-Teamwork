namespace FacebookSystem.Models
{
    using System.Collections.Generic;

    public class ApplicationUser //: IdentityUser
    {
        private ICollection<Post> posts;
        private ICollection<Group> groups;
        private ICollection<Comment> comments;
        private ICollection<ApplicationUser> friends;
        private ICollection<Notification> notifications;

        public ApplicationUser()
        {
            this.posts = new HashSet<Post>();
            this.groups = new HashSet<Group>();
            this.comments = new HashSet<Comment>();
            this.friends = new HashSet<ApplicationUser>();
            this.notifications = new HashSet<Notification>();
        }

        public virtual ICollection<Post> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public virtual ICollection<Group> Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<ApplicationUser> Friends
        {
            get { return this.friends; }
            set { this.friends = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }
    }
}
