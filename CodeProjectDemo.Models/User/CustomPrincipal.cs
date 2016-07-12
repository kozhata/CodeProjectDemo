using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.User
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly BaseUser _currentUser;

        public CustomPrincipal(BaseUser currentUser)
        {
            _currentUser = currentUser;
        }

        public IIdentity Identity
        {
            get
            {
                return _currentUser;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
