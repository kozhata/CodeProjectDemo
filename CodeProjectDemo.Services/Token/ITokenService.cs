using System.Threading.Tasks;
using CodeProjectDemo.Models.Token;
using CodeProjectDemo.Models.User;

namespace CodeProjectDemo.Services.Token
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateToken(int userId);

        bool Kill(string apiToken);

        bool Kill(int userId);

        bool ValidateToken(string apiToken);

        ApiUser GetUserByApiToken(string apiToken); 
    }
}