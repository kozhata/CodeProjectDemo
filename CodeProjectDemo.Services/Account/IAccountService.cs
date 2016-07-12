using CodeProjectDemo.Models.User;
using System.Threading.Tasks;

namespace CodeProjectDemo.Services.Account
{
    public interface IAccountService
    {
        UserDto Authenticate(string userName, string password);
    }
}