using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using Ecommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Repositories.Interfaces;
using ECommerce.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Models;

namespace ECommerce.Controllers
{

    public class HomeController : Controller
    {
        private readonly ICategory categoryRepository;
        private readonly ISprovider sproviderRepository;
        private readonly IService serviceRepository;

        public HomeController(ICategory categoryRepository, ISprovider sproviderRepository, IService serviceRepository)
        {
            this.categoryRepository = categoryRepository;
            this.sproviderRepository = sproviderRepository;
            this.serviceRepository = serviceRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.List();
            return View(categories);
        }

        public IActionResult Chat()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var Sproviders = sproviderRepository.List().Where(a => a.CompanyName.Contains(searchName));
            if (Sproviders.Count() != 0)
            {
                ViewBag.Sproviders = Sproviders;
            }
            var result = serviceRepository.List().Where(a => a.Name.Contains(searchName)).OrderBy(a=>a.Sprovider.CompanyName);
            return View(result);
        }   
    }
}
