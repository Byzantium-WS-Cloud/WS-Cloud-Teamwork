namespace FacebookSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using FacebookSystem.Models;
    using FacebookSystem.Data.Migrations;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class FacebookDbContext : IdentityDbContext<ApplicationUser>, IFacebookDbContext
    {
        public FacebookDbContext()
            : base("FacebookDbConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FacebookDbContext, Configuration>());
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Group> Groups { get; set; }
        
        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<CommentLike> CommentLikes { get; set; } 

        public virtual IDbSet<PostLike> PostLikes { get; set; }
        
        public virtual IDbSet<Notification> Notifications { get; set; }

        public virtual IDbSet<FriendRequest> FriendRequests { get; set; }

        public static FacebookDbContext Create()
        {
            return new FacebookDbContext();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Friends)
                .WithOptional()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasRequired(p => p.Owner)
                .WithMany(u => u.Posts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired<ApplicationUser>(r => r.From)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired<ApplicationUser>(r => r.To)
                .WithMany(u => u.FriendRequests)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Comment>()
            //    .HasRequired<ApplicationUser>(c => c.CommentOwner)
            //    .WithMany(u => u.Comments)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Comment>()
            //    .HasRequired(c => c.PostOwner)
            //    .WithOptional()
            //    .WillCascadeOnDelete(false);

        }
    }
}
