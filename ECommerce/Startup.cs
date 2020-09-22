using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce.Models;
using ECommerce.Models;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Repositories;
using ECommerce.Repositories.Interfaces;
using ECommerce.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ECommerce.Hubs;

namespace ECommerce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<myDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DBConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<ICategory, CategoryRepository>();
            services.AddScoped<ISprovider, SproviderRepository>();
            services.AddScoped<IService, ServiceRepository>();
            services.AddScoped<IOrder, OrderRepository>();
            services.AddScoped<IDispute, DisputeRepository>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IUserClaims, UserClaimsRepository>();
            services.AddScoped<IServiceRequested, ServiceRequestedRepository>();

            services.AddSignalR();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageCategories", policy => policy.RequireClaim("ManageCategories", "true"));
                options.AddPolicy("ManageDisputes", policy => policy.RequireClaim("ManageDisputes", "true"));
                options.AddPolicy("ManageUsers", policy => policy.RequireClaim("ManageUsers", "true"));
                options.AddPolicy("ManageOrders", policy => policy.RequireClaim("ManageOrders", "true"));
                options.AddPolicy("ManageSproviders", policy => policy.RequireClaim("ManageSproviders", "true"));
                options.AddPolicy("ManageServiceRequested", policy => policy.RequireClaim("ManageServiceRequested", "true"));
                options.AddPolicy("ManageClaims", policy => policy.RequireClaim("ManageClaims", "true"));
                options.AddPolicy("Customer", policy => policy.RequireClaim("Customer", "true"));
                options.AddPolicy("ServiceProvider", policy => policy.RequireClaim("ServiceProvider", "true"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserAndClaim(serviceProvider).Wait();
        }
        private async Task CreateUserAndClaim(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser Admin = await UserManager.FindByEmailAsync("Admin@gmail.com");
            if (Admin == null)
            {
                Admin = new ApplicationUser()
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                };
                await UserManager.CreateAsync(Admin, "P@ssw0rd");
            }
            var claimList1 = (await UserManager.GetClaimsAsync(Admin)).Select(p => p.Type);
            if (!claimList1.Contains("ManageCategories"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageCategories", "true"));
            }
            if (!claimList1.Contains("ManageOrders"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageOrders", "true"));
            }
            if (!claimList1.Contains("ManageSproviders"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageSproviders", "true"));
            }
            if (!claimList1.Contains("ManageDisputes"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageDisputes", "true"));
            }
            if (!claimList1.Contains("ManageUsers"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageUsers", "true"));
            }
            if (!claimList1.Contains("ManageServiceRequested"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageServiceRequested", "true"));
            }
            if (!claimList1.Contains("ManageClaims"))
            {
                await UserManager.AddClaimAsync(Admin, new Claim("ManageClaims", "true"));
            }

            ApplicationUser CustomerService = await UserManager.FindByEmailAsync("CustomerService@gmail.com");
            if (CustomerService == null)
            {
                CustomerService = new ApplicationUser()
                {
                    UserName = "CustomerService@gmail.com",
                    Email = "CustomerService@gmail.com",
                };
                await UserManager.CreateAsync(CustomerService, "P@ssw0rd");
            }
            var claimList2 = (await UserManager.GetClaimsAsync(CustomerService)).Select(p => p.Type);
            if (!claimList2.Contains("ManageDisputes"))
            {
                await UserManager.AddClaimAsync(CustomerService, new Claim("ManageDisputes", "true"));
            }

            ApplicationUser Customer = await UserManager.FindByEmailAsync("Customer@gmail.com");
            if (Customer == null)
            {
                Customer = new ApplicationUser()
                {
                    UserName = "Customer@gmail.com",
                    Email = "Customer@gmail.com",
                };
                await UserManager.CreateAsync(Customer, "P@ssw0rd");
            }
            var claimList4 = (await UserManager.GetClaimsAsync(Customer)).Select(p => p.Type);
            if (!claimList4.Contains("Customer"))
            {
                await UserManager.AddClaimAsync(Customer, new Claim("Customer", "true"));
            }
        }
    }
}
