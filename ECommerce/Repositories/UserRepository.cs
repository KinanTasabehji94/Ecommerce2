using Ecommerce.Models;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories
{
    public class UserRepository : IUser
    {
        myDbContext db;
        private readonly IUserClaims userClaimsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(myDbContext _db, IUserClaims userClaimsRepository, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            this.userClaimsRepository = userClaimsRepository;
            this._userManager = _userManager;
        }

        public void Add(AspNetUsers entity)
        {
            db.AspNetUsers.Add(entity);
            db.SaveChanges();
        }

        public async Task<string> Delete(string id)
        {
            var user = await Find(id);
            var userClaims = userClaimsRepository.List().Where(x => x.UserId == id);
            foreach (var item in userClaims)
            {
                userClaimsRepository.Delete(item.Id);
            }
            await _userManager.DeleteAsync(user);
            db.SaveChanges();

            return "x";
        }

        public async Task<ApplicationUser> Find(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public AspNetUsers GetUserDetails(string id)
        {
            var user =  db.AspNetUsers.Find(id);
            return user;
        }

        public IList<AspNetUsers> List()
        {
            return db.AspNetUsers.Include(d=>d.AspNetUserClaims).ToList();
        }
        public IList<AspNetUserClaims> CustomerServiceList()
        {
            return db.AspNetUserClaims.Include(d => d.User).Where(d=>d.ClaimType== "CustomerService").ToList();
        }

        public void Update(string id, AspNetUsers entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
