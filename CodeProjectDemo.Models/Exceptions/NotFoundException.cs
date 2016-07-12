using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string messageException) : base(messageException) { }

        public NotFoundException(string messageException, Exception innerException)
            : base(messageException, innerException) { }
    }
}
