using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.User
{
    public class BasicUser : BaseUser
    {
        private string _password;

        public BasicUser() : base()
        {
            _password = null;
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                _authType = "Basic";
                _isAuthenticated = true;
            }
        }
    }
}
