using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyosungManagement.Data;
using HyosungManagement.Models;
using HyosungManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HyosungManagement.Controllers
{
    public class NutritionSupportController : ViewControllerBase
    {
        public AppDbContext Context { get; }

        public NutritionSupportController(AppDbContext context)
        {
            Context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(
        //        new NutritionSupportViewModel
        //        {
        //            Services = await Context.Services.ToArrayAsync()
        //        }
        //    );
        //    ;
        //}
    }
}
