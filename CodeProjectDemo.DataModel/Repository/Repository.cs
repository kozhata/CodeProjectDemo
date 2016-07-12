namespace CodeProjectDemo.DataModel.Repository
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseTable
    {
        internal CodeProjectDemoEntities _context;
        internal DbSet<TEntity> _dbSet;
                
        public Repository(CodeProjectDemoEntities context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }     
               
        public virtual IQueryable<TEntity> All()
        {
            return _dbSet.Where(e => e.DeletedOn == null);
        }
        
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }
        
        public virtual void Insert(TEntity entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            _dbSet.Add(entity);
        }
        
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            //if (_context.Entry(entityToDelete).State == EntityState.Detached)
            //{
            //    _dbSet.Attach(entityToDelete);
            //}
            //_dbSet.Remove(entityToDelete);

            entityToDelete.DeletedOn = DateTime.UtcNow;
            _dbSet.Attach(entityToDelete);
            _context.Entry(entityToDelete).State = EntityState.Modified;
        }
        
        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}