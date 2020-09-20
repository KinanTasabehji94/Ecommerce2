using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public interface IUserClaims
    {
        IList<AspNetUserClaims> List();
        AspNetUserClaims Find(int id);
        void Add(AspNetUserClaims entity);
        void Update(int id, AspNetUserClaims entity);
        void Delete(int id);
    }
}
