using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Requests
{
    public class BooksPagedFilterRequest
    {
        public string? Date { get; set; }
        public string? Name { get; set; }
    }
}
