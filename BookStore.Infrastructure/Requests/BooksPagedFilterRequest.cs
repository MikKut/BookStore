using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Requests
{
    public class BooksPagedFilterRequest
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
