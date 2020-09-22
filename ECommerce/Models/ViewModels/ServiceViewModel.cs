using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ServiceViewModel
    {
        [Display(Name = "رقم الخدمة")]
        public int Id { get; set; }
        [Display(Name = "مقدم الخدمة")]
        public int SproviderId { get; set; }
        [Display(Name = "اسم الخدمة")]
        public string Name { get; set; }
        [Display(Name = "وصف الخدمة")]
        public string Description { get; set; }
        [Display(Name = "السعر")]
        public int? Price { get; set; }
        [Display(Name = "زمن التسليم")]
        public string ExpectedTime { get; set; }
        [Display(Name = "صورة")]
        public IFormFile Image { get; set; }

        public Sprovider Sprovider { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
