using Ecommerce.Models;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public interface IUser
    {
        IList<AspNetUsers> List();
        void Add(AspNetUsers entity);
        void Update(string id, AspNetUsers entity);
        Task<string> Delete(string id);
        Task<ApplicationUser> Find(string id);
        IList<AspNetUserClaims> CustomerServiceList();
        AspNetUsers GetUserDetails(string id);
        AspNetUsers GetUserByEmail(string Email);
    }
}
