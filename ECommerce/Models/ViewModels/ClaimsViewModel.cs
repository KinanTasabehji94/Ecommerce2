using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ClaimsViewModel
    {
        public bool Admin { get; set; }
        public bool CustomerService { get; set; }
        public bool Customer { get; set; }
        public bool ServiceProvider { get; set; }
        public bool Admin_Service { get; set; }
    }
}
