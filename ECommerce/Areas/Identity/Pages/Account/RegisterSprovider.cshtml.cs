﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ecommerce.Models;
using Ecommerce.Repositories.Interfaces;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterSprovider : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterSprovider> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISprovider _Sprovider;
        private readonly ICategory _categoryRepository;

        public RegisterSprovider(
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterSprovider> logger,
            IEmailSender emailSender,
            IHostingEnvironment hostingEnvironment,
            IServiceProvider serviceProvider,
            ISprovider Sprovider,
            ICategory categoryRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _serviceProvider = serviceProvider;
            _Sprovider = Sprovider;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "فئة مزود الخدمة")]
            public int CategoryId { get; set; }
            [Required]
            [Display(Name = "اسم مزود الخدمة")]
            public string CompanyName { get; set; }

            [Required]
            [Display(Name = "الاسم الأول")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "الاسم الاخير")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "رقم الهاتف")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "البريد الالكتروني")]
            public string Email { get; set; }

            [Display(Name = "الرقم الوطني")]
            public string NationalId { get; set; }

            [Display(Name = "المدينة")]
            public string City { get; set; }

            [Display(Name = "العنوان")]
            public string Address { get; set; }

            [Display(Name = "الموقع")]
            public string HomeLocation { get; set; }

            [Display(Name = "رابط العنوان")]
            public string MapLocation { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            [Display(Name = "تاريخ التولد")]
            public string BirthDate { get; set; }

            [Display(Name = "الجنس")]
            public string Gender { get; set; }

            [Display(Name = "شعار مزود الخدمة")]
            public IFormFile Image { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "طول كلمة المرور غير كافي، (6) احرف على الاقل", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تاكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمتي المرور غير متطابقتين")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ViewData["CategoryId"] = new SelectList(_categoryRepository.List(), "Id", "Name");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    NationalId = Input.NationalId,
                    City = Input.City,
                    Address = Input.Address,
                    HomeLocation = Input.HomeLocation,
                    BirthDate = Input.BirthDate,
                    Gender = Input.Gender,
                    PhoneNumber = Input.PhoneNumber,
                    UserType = "ServiceProvider",
                    RegisteredDate = DateTime.Now,
                    Status = "Active",
                    FullName = Input.FirstName + " " + Input.LastName
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //Add Claims To Users
                    var UserManager = _serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
                    await UserManager.AddClaimAsync(user, new Claim("ServiceProvider", "true"));

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //Add SProvider
                    string UrlImage = "";
                    var files = HttpContext.Request.Form.Files;
                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;

                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/sprovider");
                            if (file.Length > 0)
                            {
                                // var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                                var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    UrlImage = fileName;
                                }

                            }
                        }
                    }
                    Sprovider Sprovider = new Sprovider
                    {
                        UserId = user.Id,
                        Rating = 0,
                        Image = UrlImage,
                        CompanyName = Input.CompanyName,
                        CategoryId= Input.CategoryId,
                        MapLocation=Input.MapLocation

                    };
                    _Sprovider.Add(Sprovider);

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.List(), "Id", "Name");

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
