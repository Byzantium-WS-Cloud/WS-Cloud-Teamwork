namespace FacebookSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Group
    {
        private ICollection<Post> posts;
        private ICollection<ApplicationUser> members;

        public Group()
        {
            this.posts = new HashSet<Post>();
            this.members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Post> Posts 
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public virtual ICollection<ApplicationUser> Members 
        {
            get { return this.members; }
            set { this.members = value; }
        }
    }
}
