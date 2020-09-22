using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    public class DisputesController : Controller
    {
        private readonly IDispute disputeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public DisputesController(IDispute disputeRepository, IOrder orderRepository, UserManager<ApplicationUser> _userManager)
        {
            this.disputeRepository = disputeRepository;
            this._userManager = _userManager;
        }

        // GET: Disputes
        [Authorize(Policy = "ManageDisputes")]
        public ActionResult Index()
        {
            var disputes = disputeRepository.List();
            return View(disputes);
        }

        // GET: DisputesByCostomer
        [Authorize(Policy = "Customer")]
        public ActionResult CustomerDisputes(string customerId)
        {
            var disputes = disputeRepository.List().Where(x=>x.Order.CustomerId == customerId);
            return View(disputes);
        }

        // GET: DisputesByCompany
        [Authorize(Policy = "ServiceProvider")]
        public ActionResult CompanyDisputes(string SproviderId)
        {
            var disputes = disputeRepository.List().Where(x=>x.Order.Service.Sprovider.User.Id == SproviderId);
            return View(disputes);
        }

        // GET: Disputes/Details/5
        public ActionResult Details(int id)
        {
            var dispute = disputeRepository.Find(id);

            return View(dispute);
        }

        // GET: Disputes/Create
        [Authorize(Policy = "Customer")]
        public ActionResult Create(int orderId, string customerId)
        {
            ViewBag.CustomerId = customerId;
            TempData["OrderId"] = orderId;
            return View();
        }

        // POST: Disputes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult Create(Dispute dispute)
        {
            dispute.OrderId = Int32.Parse(TempData["OrderId"].ToString());
            dispute.OpenedDate = DateTime.Now;
            dispute.Status = "Opened";
            if (ModelState.IsValid)
            {
                disputeRepository.Add(dispute);

                return RedirectToAction(nameof(CustomerDisputes), new { customerId = _userManager.GetUserId(HttpContext.User) });
            }
            return View();
        }

        [Authorize(Policy = "ManageDisputes")]
        public ActionResult TakeDispute(int id, string arbiterId)
        {
            var dispute = disputeRepository.Find(id);
            dispute.Status = "OnProgress";
            dispute.ArbiterId = arbiterId;
            disputeRepository.Update(id, dispute);

            return RedirectToAction(nameof(Index));
        }

        // GET: Disputes/Edit/5
        [Authorize(Policy = "ManageDisputes")]
        public ActionResult Edit(int id)
        {
            var dispute = disputeRepository.Find(id);
            return View(dispute);
        }

        // POST: Disputes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageDisputes")]
        public ActionResult Edit(int id, [Bind("Id,OrderId,OpenedDate,ArbiterId,Result,Status")] Dispute dispute)
        {
            if (ModelState.IsValid)
            {
                Dispute newDispute = disputeRepository.Find(id);

                newDispute.Result = dispute.Result;
                newDispute.Status = dispute.Status;

                disputeRepository.Update(id, newDispute);

                return RedirectToAction(nameof(Index));
            }
            return View(dispute);
        }


        // GET: Orders/Delete/5
        [Authorize(Policy = "Customer")]
        public ActionResult Delete(int id)
        {
            var dispute = disputeRepository.Find(id);
            if (dispute == null)
            {
                return NotFound();
            }

            return View(dispute);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dispute = disputeRepository.Find(id);
            disputeRepository.Delete(id);
            return RedirectToAction(nameof(CustomerDisputes), new { customerId = _userManager.GetUserId(HttpContext.User) });
        }
    }
}
