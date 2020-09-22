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

        public IActionResult Chat()
        {
            return View();
        }

        [Authorize(Policy = "ManageClaims")]
        public IActionResult IndexCS()
        {
            var Users = userRepository.CustomerServiceList();
            return View(Users);
        }

        // GET: Categories/Edit/5
        [Authorize(Policy = "ManageClaims")]
        public ActionResult EditClaims(string UserId)
        {
            ViewBag.User = userRepository.GetUserDetails(UserId);
            ViewBag.UserClaims = userClaimsRepository.List().Where(d => d.UserId == UserId);
         
            return View();
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageClaims")]
        public async Task<ActionResult> EditClaims(bool ManageCategories, bool ManageUsers, bool ManageOrders, bool ManageSproviders, bool ManageServiceRequested, bool ManageClaims, string UserID)
        {
            var UserManager = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();

            var user = await userRepository.Find(UserID);
            var oldClaims = userClaimsRepository.List().Where(x => x.UserId == UserID);
            foreach (var item in oldClaims)
            {
                await UserManager.RemoveClaimAsync(user, new Claim(item.ClaimType, "true"));
            }

            await UserManager.AddClaimAsync(user, new Claim("ManageDisputes", "true"));

            if (ManageCategories == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageCategories", "true"));
            }

            if (ManageUsers == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageUsers", "true"));
            }

            if (ManageOrders == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageOrders", "true"));
            }

            if (ManageSproviders == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageSproviders", "true"));
            }

            if (ManageServiceRequested == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageServiceRequested", "true"));
            }

            if (ManageClaims == true)
            {
                await UserManager.AddClaimAsync(user, new Claim("ManageClaims", "true"));
            }

            return RedirectToAction(nameof(IndexCS));
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
