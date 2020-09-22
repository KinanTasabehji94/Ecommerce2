using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class ServiceRequestedController : Controller
    {
        private readonly IServiceRequested serviceRequestedRepository;

        public ServiceRequestedController(IServiceRequested serviceRequestedRepository)
        {
            this.serviceRequestedRepository = serviceRequestedRepository;
        }
        // GET: ServiceRequestedController
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult Index()
        {
            var requested = serviceRequestedRepository.List();
            return View(requested);
        }

        // GET: ServiceRequestedController/Details/5
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult Details(int id)
        {
            var service = serviceRequestedRepository.Find(id);
            return View(service);
        }

        // GET: ServiceRequestedController/Create
         [Authorize(Policy = "Customer")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestedController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Customer")]
        public ActionResult Create(ServiceRequested service)
        {
            if (ModelState.IsValid)
            {
                serviceRequestedRepository.Add(service);

                return RedirectToAction("Index", "Home");

            }
            return View();
        }

        // GET: ServiceRequestedController/Edit/5
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult Edit(int id)
        {
            var service = serviceRequestedRepository.Find(id);
            return View(service);
        }

        // POST: ServiceRequestedController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult Edit(int id, ServiceRequested service)
        {
            if (ModelState.IsValid)
            {
                ServiceRequested newServiceRequested = serviceRequestedRepository.Find(id);

                newServiceRequested.Name = service.Name;
                newServiceRequested.Describtion = service.Describtion;

                serviceRequestedRepository.Update(newServiceRequested);

                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: ServiceRequestedController/Delete/5
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult Delete(int id)
        {
            var service = serviceRequestedRepository.Find(id);
            return View(service);
        }

        // POST: ServiceRequestedController/Delete/5
        [HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageServiceRequested")]
        public ActionResult DeleteConfirmed(int id)
        {
            serviceRequestedRepository.Delete(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
