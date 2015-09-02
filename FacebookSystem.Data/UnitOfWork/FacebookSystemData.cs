namespace FacebookSystem.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;

    using Repository;
    using FacebookSystem.Models;

    public class FacebookSystemData : IFacebookSystemData
    {
        private IFacebookDbContext context;
        private IDictionary<Type, object> repositories;

        public FacebookSystemData()
            : this(new FacebookDbContext())
        {
        }

        public FacebookSystemData(IFacebookDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<PostLike> PostLikes
        {
            get { return GetRepository<PostLike>(); }
        }

        public IGenericRepository<Post> Posts
        {
            get { return GetRepository<Post>(); }
        }

        public IGenericRepository<Comment> Comments
        {
            get { return GetRepository<Comment>(); }
        }

        public IGenericRepository<CommentLike> CommentLikes
        {
            get {  return GetRepository<CommentLike>(); }
        } 

        public IGenericRepository<Group> Groups
        {
            get { return GetRepository<Group>(); }
        }

        public IGenericRepository<Notification> Notifications
        {
            get { return GetRepository<Notification>(); }
        }

        public IGenericRepository<ApplicationUser> ApplicationUsers
        {
            get { return GetRepository<ApplicationUser>(); }
        }

        public IGenericRepository<FriendRequest> FriendRequests
        {
            get { return GetRepository<FriendRequest>(); }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var modelType = typeof(T);
            if (!this.repositories.ContainsKey(modelType))
            {
                var repositoryType = typeof(GenericRepository<T>);
                this.repositories.Add(modelType, Activator.CreateInstance(repositoryType, this.context));
            }

            return (IGenericRepository<T>)this.repositories[modelType];
        }
    }
}
