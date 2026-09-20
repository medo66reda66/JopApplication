using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Dtos
{
    public class ShowMyApp
    {
        public string CVUrl { get; set; } = string.Empty;
        public string GitUrl { get; set; } = string.Empty;
        public string LinkdenUrl { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public JopStatus jopStatus { get; set; }
        public string? ImageUrl { get; set; }
    }
}
