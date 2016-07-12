using CodeProjectDemo.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.User
{
    public class ApiUser : BaseUser
    {
        private RolesEnum _roles;
        private string _apiToken;

        public ApiUser() : base()
        {
            _roles = RolesEnum.None;
            _apiToken = null;
        }

        public RolesEnum Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                _roles = value;
            }
        }

        public string ApiToken
        {
            get
            {
                return _apiToken;
            }
            set
            {
                _apiToken = value;
                _isAuthenticated = true;
                _authType = "Token";
            }
        }
    }
}
