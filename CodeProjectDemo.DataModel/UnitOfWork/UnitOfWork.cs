namespace CodeProjectDemo.DataModel.UnitOfWork
{
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly CodeProjectDemoEntities _context;
        private IRepository<User> _users;
        private IRepository<Product> _products;
        private IRepository<Token> _tokens;

        public UnitOfWork(CodeProjectDemoEntities context)
        {
            _context = context;
        }
        
        public IRepository<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = new Repository<Product>(_context);
                }
                return _products;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new Repository<User>(_context);
                }
                return _users;
            }
        }
        public IRepository<Token> Tokens
        {
            get
            {
                if (_tokens == null)
                {
                    _tokens = new Repository<Token>(_context);
                }
                return _tokens;
            }
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }
        
        private bool _disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}