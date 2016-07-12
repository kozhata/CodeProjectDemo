using CodeProjectDemo.DataModel;
using CodeProjectDemo.DataModel.UnitOfWork;
using CodeProjectDemo.Models.Exceptions;
using CodeProjectDemo.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDto Authenticate(string userName, string password)
        {
            User user = _unitOfWork.Users.All()
                .Where(u =>
                            u.UserName == userName && u.Password == password)
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                UserName = user.UserName
            };
        }
    }
}
