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

namespace ECommerce.Controllers
{

    public class HomeController : Controller
    {
        private readonly ICategory categoryRepository;
        private readonly IService serviceRepository;
        private readonly ISprovider sproviderRepository;

        public HomeController(ICategory categoryRepository, IService serviceRepository, ISprovider sproviderRepository)
        {
            this.categoryRepository = categoryRepository;
            this.serviceRepository = serviceRepository;
            this.sproviderRepository = sproviderRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.List();
            return View(categories);
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
