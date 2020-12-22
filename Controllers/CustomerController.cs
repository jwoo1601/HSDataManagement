using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using HyosungManagement.Data;
using HyosungManagement.InputModels;
using HyosungManagement.Models;
using HyosungManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HyosungManagement.Controllers
{
    public class CustomerController : ViewControllerBase
    {
        private AppDbContext Context { get; }
        private ILogger<Customer> Logger { get; }

        public CustomerController(
            AppDbContext context,
            ILogger<Customer> logger
        )
        {
            Context = context;
            Logger = logger;
        }

        private class SortingCriteria
        {
            public static string Number => "number";
            public static string Name => "name";
        }

        private class SortingOrder
        {
            public static string Ascending => "ascending";
            public static string Descending => "descending";
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(
        //        new CustomerViewModel
        //        {
        //            NumCustomers = await Context.Customers.CountAsync(),
        //            NumItemsPerPage = 10
        //        }
        //    );
        //}

        public async Task<IActionResult> Display(
            [FromQuery(Name = "page")] string pageString,
            [FromQuery(Name = "limit")] string limitString,
            [FromQuery(Name = "criteria")] string criteria,
            [FromQuery(Name = "order")] string order
        )
        {
            int page;
            if (!int.TryParse(pageString, out page))
            {
                page = 1;
            }

            int numItemsPerPage;
            if (!int.TryParse(limitString, out numItemsPerPage))
            {
                numItemsPerPage = 10;
            }

            if (
                criteria != SortingCriteria.Number &&
                criteria != SortingCriteria.Name
            )
            {
                criteria = SortingCriteria.Number;
            }

            if (
                order != SortingOrder.Ascending &&
                order != SortingOrder.Descending
            )
            {
                order = SortingOrder.Ascending;
            }

            var customers = await Context.Customers
                                        
                                        .ToArrayAsync();

            var customerTags = new Dictionary<int, IEnumerable<CustomerTag>>(customers.Length);
            foreach (var customer in customers)
            {
                var tagList = await Context.CustomerTagAssignments
                                            .Include(a => a.Customer)
                                            .Include(a => a.Tag)
                                            
                                            .Where(a => a.CustomerID == customer.ID)
                                            .Select(a => a.Tag)
                                            .ToListAsync();

                customerTags[customer.ID] = tagList;
            }

            var sortedCustomers =
                order == SortingOrder.Ascending ?
                    (
                        criteria == SortingCriteria.Number ?
                        customers.OrderBy(c => c.ID) :
                        customers.OrderBy(c => c.Name)
                    ) :
                    (
                        criteria == SortingCriteria.Number ?
                        customers.OrderByDescending(c => c.ID) :
                        customers.OrderByDescending(c => c.Name)
                    );


            return PartialView(
                "_CustomerTableBody",
                new CustomerTableBodyViewModel
                {
                    Customers = sortedCustomers,
                    CustomerTags = customerTags,
                    Page = page,
                    NumItemsPerPage = numItemsPerPage
                }
            );
        }


    }
}
