using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class CustomerTableBodyViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
            = Enumerable.Empty<Customer>();
        public IDictionary<int, IEnumerable<CustomerTag>> CustomerTags { get; set; }
        public int Page { get; set; }
        public int NumItemsPerPage { get; set; }
    }
}
