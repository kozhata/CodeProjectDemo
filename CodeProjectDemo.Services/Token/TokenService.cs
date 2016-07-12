namespace CodeProjectDemo.Services.Token
{
    using DataModel.UnitOfWork;
    using Models.Token;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.User;

    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenDto> GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString().ToLowerInvariant();
            DateTime createdOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddMinutes(60);

            DataModel.Token dbToken = new DataModel.Token
            {
                UserFk = userId,
                AuthToken = token,
                CreatedOn = createdOn,
                ExpiresOn = expiredOn,
                Roles = (int)RolesEnum.LevelTwo
            };

            _unitOfWork.Tokens.Insert(dbToken);
            await _unitOfWork.SaveAsync();

            TokenDto tokenDto = new TokenDto
            {
                Id = dbToken.Id,
                UserFk = userId,
                CreatedOn = createdOn,
                ExpiresOn = expiredOn,
                AuthToken = token
            };

            return tokenDto;
        }

        public bool ValidateToken(string apiToken)
        {
            DataModel.Token dbToken = _unitOfWork.Tokens.All()
                .Where(t =>
                            t.AuthToken == apiToken
                            && t.ExpiresOn > DateTime.Now)
                .FirstOrDefault();

            if (dbToken != null)
            {
                dbToken.ExpiresOn = dbToken.ExpiresOn.AddMinutes(60);

                _unitOfWork.Tokens.Update(dbToken);
                _unitOfWork.SaveAsync();

                return true;
            }
            return false;
        }

        public bool Kill(string apiToken)
        {
            DataModel.Token dbToken = _unitOfWork.Tokens.All()
                .Where(t => t.AuthToken == apiToken)
                .FirstOrDefault();

            _unitOfWork.Tokens.Delete(dbToken);

            _unitOfWork.SaveAsync();

            bool isNotDeleted = _unitOfWork.Tokens.All()
                .Where(x => x.AuthToken == apiToken)
                .Any();

            if (isNotDeleted)
            {
                return false;
            }

            return true;
        }

        public bool Kill(int userId)
        {
            DataModel.Token dbToken = _unitOfWork.Tokens.All()
                .Where(t => t.UserFk == userId)
                .FirstOrDefault();

            _unitOfWork.Tokens.Delete(dbToken);

            bool isNotDeleted = _unitOfWork.Tokens.All().
                Where(x => x.UserFk == userId)
                .Any();

            if (isNotDeleted)
            {
                return false;
            }

            return true;
        }

        public ApiUser GetUserByApiToken(string apiToken)
        {
            ApiUser user = _unitOfWork.Tokens.All()
                .Include(t => t.User)
                .Where(t => t.AuthToken == apiToken
                             && t.ExpiresOn > DateTime.Now)
                .Select(t => new ApiUser()
                {
                    Id = t.User.Id,
                    Roles = (RolesEnum)t.Roles,
                    ApiToken = t.AuthToken,
                    UserName = t.User.Name
                })
                .FirstOrDefault();

            return user;
        }
    }
}
