using CodeProjectDemo.Models.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string AuthorizationHeader
        {
            get
            {
                return Global.ResourceManager.GetString("Authorization");
            }
        }

        public string TokenHeader
        {
            get
            {
                return Global.ResourceManager.GetString("Token");
            }
        }
    }
}
