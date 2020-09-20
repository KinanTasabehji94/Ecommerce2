using Ecommerce.Models;
using Ecommerce.Repositories.Interfaces;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class ServiceRepository:IService
    {

        myDbContext db;
        private readonly IOrder orderRepository;

        public ServiceRepository(myDbContext _db, IOrder orderRepository)
        {
            db = _db;
            this.orderRepository = orderRepository;
        }

        public void Add(Service entity)
        {
            db.Service.Add(entity);
            db.SaveChanges();
        }

        public void Update(int id, Service entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var service = Find(id);

            var orders = orderRepository.List().Where(x => x.ServiceId == id);
            foreach (var item in orders)
            {
                orderRepository.Delete(item.Id);
            }

            db.Service.Remove(service);
            db.SaveChanges();
        }
        public IList<Service> List()
        {
            return db.Service.Include(s => s.Sprovider).ThenInclude(s => s.User).ToList();
        }

        public Service Find(int id)
        {
            var service = db.Service
                .Include(s => s.Sprovider)
                .ThenInclude(s=>s.User)
                .FirstOrDefault(s => s.Id == id);
            return service;
        }

        public List<Service> Search(string term)
        {
            return db.Service.Where(a => a.Name.Contains(term)).ToList();
        }
    }
}
