using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeProjectDemo.Models.RestBase
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }

        public int TotalRecords { get; set; }

        public int TotalDisplayRecords { get; set; }
    }
}
