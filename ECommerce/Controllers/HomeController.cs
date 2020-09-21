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
        private readonly IServiceProvider serviceProvider;
        private readonly IService serviceRepository;
        private readonly ISprovider sproviderRepository;
        private readonly IUser userRepository;
        private readonly IUserClaims userClaimsRepository;

        public HomeController(ICategory categoryRepository, IServiceProvider serviceProvider, IService serviceRepository, ISprovider sproviderRepository, IUser userRepository, IUserClaims userClaimsRepository)
        {
            this.categoryRepository = categoryRepository;
            this.serviceProvider = serviceProvider;
            this.serviceRepository = serviceRepository;
            this.sproviderRepository = sproviderRepository;
            this.userRepository = userRepository;
            this.userClaimsRepository = userClaimsRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.List();
            return View(categories);
        }

        public IActionResult CustomerServers()
        {
            var Users = userRepository.CustomerServiceList();
            return View(Users);
        }


        // GET: Categories/Edit/5
       // [Authorize(Policy = "Admin")]
        public ActionResult Edit(string UserId)
        {
            ViewBag.User = userRepository.GetUserDetails(UserId);
            ViewBag.UserClaims = userClaimsRepository.List().Where(d => d.UserId == UserId);
         
            return View();
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "Admin")]
        public async Task<ActionResult> Edit(bool Admin, bool CustomerService, bool Customer, bool ServiceProvider, bool Admin_Service , string UserID)
        {
            var UserManager = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();

            var user = await userRepository.Find(UserID);
            if (Admin==true)
            {
                await UserManager.AddClaimAsync(user, new Claim("Admin", "true"));
            }
            else
            {
                await UserManager.RemoveClaimAsync(user, new Claim("Admin", "true"));
            }
            if (CustomerService == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("CustomerService", "true"));
            }
            else
            {
                await UserManager.RemoveClaimAsync(user, new Claim("CustomerService", "true"));
            }
            if (Customer == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("Customer", "true"));
            }
            else
            {
                await UserManager.RemoveClaimAsync(user, new Claim("Customer", "true"));
            }
            if (ServiceProvider == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ServiceProvider", "true"));
            }
            else
            {
                await UserManager.RemoveClaimAsync(user, new Claim("ServiceProvider", "true"));
            }
            if (Admin_Service == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("Admin_CustomerService", "true"));
            }
            else
            {
                await UserManager.RemoveClaimAsync(user, new Claim("Admin_CustomerService", "true"));
            }
            return RedirectToAction(nameof(CustomerServers));
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
