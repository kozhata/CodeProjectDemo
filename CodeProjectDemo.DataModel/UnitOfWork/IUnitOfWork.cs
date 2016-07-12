using CodeProjectDemo.DataModel.Repository;
using System.Threading.Tasks;

namespace CodeProjectDemo.DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Product> Products { get; }

        IRepository<Token> Tokens { get; }

        IRepository<User> Users { get; }

        void Dispose();

        Task<int> SaveAsync();
    }
}