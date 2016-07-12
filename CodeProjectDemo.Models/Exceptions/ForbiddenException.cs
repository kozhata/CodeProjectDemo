using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }

        public ForbiddenException(string messageException) : base(messageException) { }

        public ForbiddenException(string messageException, Exception innerException)
            : base(messageException, innerException) { }
    }
}
