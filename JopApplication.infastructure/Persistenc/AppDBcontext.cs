using JopApplication.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.infrastructure.Persistenc
{
    public class AppDBcontext : IdentityDbContext<Appuser>
    {
        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {
        }
        public DbSet<Jop> Jops { get; set; }
        public DbSet<CandidatJopApplication> CandidatJopApplications { get; set; }
        public DbSet<Candidate> candidates { get; set; }

    }
}
