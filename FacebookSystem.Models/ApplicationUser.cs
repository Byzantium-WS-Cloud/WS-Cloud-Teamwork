namespace FacebookSystem.Models
{
    using System.Threading.Tasks;
    using System.Security.Claims;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Enums;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Post> posts;
        private ICollection<Group> groups;
        private ICollection<Comment> comments;
        private ICollection<ApplicationUser> friends;
        private ICollection<Notification> notifications;
        private ICollection<FriendRequest> friendRequests;

        public ApplicationUser()
        {
            this.posts = new HashSet<Post>();
            this.groups = new HashSet<Group>();
            this.comments = new HashSet<Comment>();
            this.friends = new HashSet<ApplicationUser>();
            this.notifications = new HashSet<Notification>();
            this.friendRequests = new HashSet<FriendRequest>();
        }
        
        public string Name { get; set; }

        public string ProfileImageData { get; set; }

        public string ProfileImageDataMinified { get; set; }

        public string CoverImageData { get; set; }

        public Gender Gender { get; set; }

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

        public virtual ICollection<FriendRequest> FriendRequests
        {
            get { return this.friendRequests; }
            set { this.friendRequests = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}
