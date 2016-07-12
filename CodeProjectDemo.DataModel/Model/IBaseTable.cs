using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.DataModel.Model
{
    public interface IBaseTable
    {
        int Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
