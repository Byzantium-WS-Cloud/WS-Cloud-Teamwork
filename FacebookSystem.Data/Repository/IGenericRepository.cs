namespace FacebookSystem.Data.Repository
{
    using System.Linq;
    using System.Data.Entity;

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> All();

        void Add(T entity);

        void Edit(T entity);

        void Delete(T entity);

        void Detach(T entity);

        void ChangeState(T entity, EntityState state);
    }
}
