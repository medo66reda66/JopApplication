using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Services
{
    public class CloseJopRecurringJobs : ICloseJopRecurringJobs
    {
        private readonly IRepository<Jop> _Joprepository;

        public CloseJopRecurringJobs(IRepository<Jop> joprepository)
        {
            _Joprepository = joprepository;
        }

        public async Task<IEnumerable<Jop>> CloseJop()
        {
            var jobs = await _Joprepository.GetAllAsync(
                    j => j.jopStatus == JopStatus.Open &&
                         j.PostedDate <= DateTime.UtcNow.AddMonths(-6)
                );

            foreach (var job in jobs)
            {
                job.jopStatus = JopStatus.Closed;
            }

            await _Joprepository.SaveChangesAsync();

            if (jobs == null)
            {
                return null;
            }

            return jobs;
        }
    }
}
