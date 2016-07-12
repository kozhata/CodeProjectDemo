namespace CodeProjectDemo.DataModel.Repository
{
    using Model;
    using System.Linq;

    public interface IRepository<TEntity> where TEntity : IBaseTable
    {
        IQueryable<TEntity> All();

        void Delete(TEntity entityToDelete);

        void Delete(object id);

        TEntity GetById(object id);

        void Insert(TEntity entity);

        void Update(TEntity entityToUpdate);
    }
}