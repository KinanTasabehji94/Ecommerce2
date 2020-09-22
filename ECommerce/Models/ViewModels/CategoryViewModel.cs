using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "فئة الخدمة")]
        public string Name { get; set; }
        [Display(Name = "وصف الفئة")]
        public string Description { get; set; }
        [Display(Name = "صورة")]
        public IFormFile Image { get; set; }
    }
}
