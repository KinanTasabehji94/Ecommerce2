using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            Dispute = new HashSet<Dispute>();
            Order = new HashSet<Order>();
            Sprovider = new HashSet<Sprovider>();
        }

        public string Id { get; set; }
        [Display(Name ="اسم المستخدم")]
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Display(Name = "الاسم الاول")]
        public string FirstName { get; set; }
        [Display(Name = "الكنية")]
        public string LastName { get; set; }
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }
        [Display(Name = "تاريخ الميلاد")]
        public string BirthDate { get; set; }
        public string NationalId { get; set; }
        [Display(Name = "المدينة")]
        public string City { get; set; }
        [Display(Name = "العنوان")]
        public string Address { get; set; }
        [Display(Name = "الموقع")]
        public string HomeLocation { get; set; }
        [Display(Name = "الجنس")]
        public string Gender { get; set; }
        [Display(Name = "تاريخ الانضمام")]
        public DateTime RegisteredDate { get; set; }
        [Display(Name = "الحالة")]
        public string Status { get; set; }
        [Display(Name = "نوع المستخدم")]
        public string UserType { get; set; }

        public ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public ICollection<Dispute> Dispute { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<Sprovider> Sprovider { get; set; }
    }
}
