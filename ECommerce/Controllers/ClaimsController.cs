using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Models;
using Ecommerce.Repositories.Interfaces;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUser userRepository;
        private readonly IUserClaims userClaimsRepository;

        public ClaimsController(IServiceProvider serviceProvider, IUser userRepository, IUserClaims userClaimsRepository)
        {
            this.serviceProvider = serviceProvider;
            this.userRepository = userRepository;
            this.userClaimsRepository = userClaimsRepository;
        }
        [Authorize(Policy = "ManageClaims")]
        public IActionResult Index()
        {
            var Users = userRepository.CustomerServiceList();
            return View(Users);
        }

        // GET: Categories/Edit/5
        [Authorize(Policy = "ManageClaims")]
        public ActionResult Edit(string UserId)
        {
            ViewBag.User = userRepository.GetUserDetails(UserId);
            ViewBag.UserClaims = userClaimsRepository.List().Where(d => d.UserId == UserId);

            return View();
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageClaims")]
        public async Task<ActionResult> Edit(bool ManageCategories, bool ManageUsers, bool ManageOrders, bool ManageSproviders, bool ManageServiceRequested, bool ManageClaims, string UserID)
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

            return RedirectToAction(nameof(Index));
        }

    }
}
