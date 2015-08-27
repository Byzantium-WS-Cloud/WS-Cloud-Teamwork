namespace FacebookSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using FacebookSystem.Models;
    
    public interface IFacebookDbContext
    {
        IDbSet<Post> Posts { get; set; }

        IDbSet<Group> Groups { get; set; }
        
        IDbSet<Comment> Comments { get; set; }

        IDbSet<PostLike> PostLikes { get; set; }
        
        IDbSet<Notification> Notifications { get; set; }

        IDbSet<FriendRequest> FriendRequests { get; set; }

        void SaveChanges();

        IDbSet<T> Set<T>() where T : class;
        
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
