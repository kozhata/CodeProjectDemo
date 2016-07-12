using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.User
{
    public class BaseUser : IIdentity
    {
        private string _name;
        protected string _authType;
        protected bool _isAuthenticated;
        private int _id;
        private string _userName;

        public BaseUser()
        {
            _isAuthenticated = false;
            _id = 0;
            _name = null;
            _authType = null;
            _userName = null;
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                _name = value;
            }
        }
        
        public string Name { get { return _name; } }

        public string AuthenticationType { get { return _authType; } }

        public bool IsAuthenticated { get { return _isAuthenticated; } }
    }
}
