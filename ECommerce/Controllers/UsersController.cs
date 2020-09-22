using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUser userRepository;

        public UsersController(IUser userRepository)
        {
            this.userRepository = userRepository;
        }
        [Authorize(Policy = "ManageUsers")]
        public IActionResult Index()
        {
            var users = userRepository.List();
            return View(users);
        }

        [Authorize(Policy = "ManageUsers")]
        public ActionResult Block(string userId)
        {
            var user = userRepository.GetUserDetails(userId);
            user.Status = "Blocked";
            userRepository.Update(userId, user);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "ManageUsers")]
        public ActionResult UnBlock(string userId)
        {
            var user = userRepository.GetUserDetails(userId);
            user.Status = "Active";
            userRepository.Update(userId, user);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(string id)
        {
            var user = userRepository.GetUserDetails(id);

            return View(user);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            userRepository.Delete(id);
            return View();
        }

    }
}
