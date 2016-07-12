using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.Token
{
    [Flags]
    public enum RolesEnum : int
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,

        LevelOne = Read,
        LevelTwo = Read | Write,
        LevelThree = Read | Write | Delete
    }
}
