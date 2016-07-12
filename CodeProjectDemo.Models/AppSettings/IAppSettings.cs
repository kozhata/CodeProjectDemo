using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.AppSettings
{
    public interface IAppSettings
    {
        string AuthorizationHeader { get; }

        string TokenHeader { get; }
    }
}
