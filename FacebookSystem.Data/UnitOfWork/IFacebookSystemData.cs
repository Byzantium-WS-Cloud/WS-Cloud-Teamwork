namespace FacebookSystem.Data.UnitOfWork
{
    using FacebookSystem.Models;
    using FacebookSystem.Data.Repository;

    public interface IFacebookSystemData
    {
        IGenericRepository<Post> Posts { get; }

        IGenericRepository<Comment> Comments { get; }

        IGenericRepository<Group> Groups { get; }

        IGenericRepository<PostLike> PostLikes { get; }

        IGenericRepository<Notification> Notifications { get; }

        void SaveChanges();
    }
}
