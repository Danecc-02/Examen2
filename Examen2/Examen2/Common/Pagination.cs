using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2.Common
{
    public class Pagination<T> where T : class
        {

            public int CurrentPage { get; set; }

            public int RecordsPerPage { get; set; }

            public int TotalRecords { get; set; }

            public int TotalPages { get; set; }
            public string Search { get; set; }
            public IEnumerable<T> Result { get; set; }
        


    }
}
