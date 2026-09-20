using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Dtos
{
    public class ShowAllApplications
    {
        // Application
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime AppliedAt { get; set; }
        public Applicationstatuse Applicationstatuse { get; set; }

        // Candidate
        public string CVUrl { get; set; } = string.Empty;
        public string GitUrl { get; set; } = string.Empty;
        public string LinkdenUrl { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;

        // User
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Job
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public JopStatus JopStatus { get; set; }
        public string? ImageUrl { get; set; }
    }
}
