using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public class UserClaimsRepository : IUserClaims
    {
        myDbContext db;
        public UserClaimsRepository(myDbContext _db)
        {
            db = _db;
        }

        public void Add(AspNetUserClaims entity)
        {
            db.AspNetUserClaims.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var aspNetUserClaims = Find(id);
            db.AspNetUserClaims.Remove(aspNetUserClaims);
            db.SaveChanges();
        }

        public AspNetUserClaims Find(int id)
        {
            var aspNetUserClaims = db.AspNetUserClaims.SingleOrDefault(a => a.Id == id);

            return aspNetUserClaims;
        }

        public IList<AspNetUserClaims> List()
        {
            return db.AspNetUserClaims.ToList();
        }

        public void Update(int id, AspNetUserClaims entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
