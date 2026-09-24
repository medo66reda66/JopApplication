using JopApplication.Application.Services;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface ICloseJopRecurringJobs : CloseJopRecurringJobs
    {
          Task<IEnumerable<Jop>> CloseJop();
    }
}
