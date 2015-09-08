namespace FacebookSystem.Data.Repository
{
    using System.Linq;
    using System.Data.Entity;

    using FacebookSystem.Data;
    using FacebookSystem.Data.Repository;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IFacebookDbContext context;
        private IDbSet<T> set;

        public GenericRepository()
            : this(new FacebookDbContext())
        {
        }

        public GenericRepository(IFacebookDbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set;
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Edit(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Detach(T entity)
        {
            this.ChangeState(entity, EntityState.Detached);
        }


        public void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (EntityState.Detached == state)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
