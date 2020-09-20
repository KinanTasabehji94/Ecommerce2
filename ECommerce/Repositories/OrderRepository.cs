using Ecommerce.Repositories.Interfaces;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories
{
    public class OrderRepository : IOrder
    {
        myDbContext db;
        private readonly IDispute disputeRepository;

        public OrderRepository(myDbContext _db, IDispute disputeRepository)
        {
           
            db = _db;
            this.disputeRepository = disputeRepository;
        }
        public void Add(Order entity)
        {
            db.Order.Add(entity);
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            var order = Find(id);
            var disputes = disputeRepository.List().Where(x => x.OrderId == id);
            foreach (var item in disputes)
            {
                disputeRepository.Delete(item.Id);
            }
            db.Order.Remove(order);
            db.SaveChanges();
        }
        public Order Find(int id)
        {
            var order = db.Order
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .ThenInclude(o => o.Sprovider).
            ThenInclude(o => o.User)
            .ThenInclude(o => o.Sprovider)
            .ThenInclude(o => o.Category)
            .FirstOrDefault(o => o.Id == id);
            return order;
        }
        public IList<Order> List()
        {
            return db.Order.Include(o => o.Customer).Include(o => o.Service).ThenInclude(o => o.Sprovider).ThenInclude(o => o.User).ThenInclude(o => o.Sprovider).ThenInclude(o => o.Category).ToList();

        }
        public void Update(int id, Order entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }

    }
}
