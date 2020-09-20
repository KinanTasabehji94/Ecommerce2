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
    public class SproviderRepository : ISprovider
    {
        myDbContext db;
        private readonly IService serviceRepository;
        private readonly IUser userRepository;

        public SproviderRepository(myDbContext _db, IService serviceRepository, IUser userRepository)
        {
            db = _db;
            this.serviceRepository = serviceRepository;
            this.userRepository = userRepository;
        }
        public void Add(Sprovider entity)
        {
            db.Sprovider.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ServicesProvider = Find(id);
            var services = serviceRepository.List().Where(x => x.SproviderId == id);
            foreach (var item in services)
            {
                serviceRepository.Delete(item.Id);
            }

            userRepository.Delete(ServicesProvider.UserId);

            db.Sprovider.Remove(ServicesProvider);
            db.SaveChanges();
        }

        public Sprovider Find(int id)
        {
            var Sprovider = db.Sprovider.Include(s => s.Category).Include(s => s.User).SingleOrDefault(a => a.Id == id);

            return Sprovider;
        }

        public IList<Sprovider> List()
        {
            return db.Sprovider.Include(s => s.Category).Include(s => s.User).ToList();
        }

        public void Update(int id, Sprovider entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
