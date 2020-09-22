using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories
{
    public class ServiceRequestedRepository: IServiceRequested
    {

        myDbContext db;

        public ServiceRequestedRepository(myDbContext _db)
        {
            db = _db;
        }

        public void Add(ServiceRequested entity)
        {
            db.ServiceRequested.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var serviceRequested = Find(id);

            db.ServiceRequested.Remove(serviceRequested);
            db.SaveChanges();
        }

        public ServiceRequested Find(int id)
        {
            var serviceRequested = db.ServiceRequested.SingleOrDefault(s=>s.Id==id);

            return serviceRequested;
        }

        public IList<ServiceRequested> List()
        {
            return db.ServiceRequested.ToList();
        }

        public void Update(ServiceRequested entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
