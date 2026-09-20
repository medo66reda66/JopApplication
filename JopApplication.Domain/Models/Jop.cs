using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Domain.Models
{
    public enum JopStatus
    {
        Open,
        Closed,
    }
    public class Jop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public JopStatus jopStatus { get; set; }
        public string? ImageUrl { get; set; }
        public string AppuserId { get; set; }
        public Appuser? Appuser { get; set; }

    }
}
