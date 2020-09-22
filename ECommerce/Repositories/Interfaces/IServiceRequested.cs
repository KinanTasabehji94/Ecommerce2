using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Repositories.Interfaces
{
    public interface IServiceRequested
    {
        void Add(ServiceRequested entity);
        void Delete(int id);
        ServiceRequested Find(int id);
        IList<ServiceRequested> List();
        void Update(ServiceRequested entity);
    }
}
