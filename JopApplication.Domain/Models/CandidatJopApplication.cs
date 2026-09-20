using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Domain.Models
{
    public enum Applicationstatuse
    {
        Applied,
        UnderReview,
        Interview,
        Accepted,
        Rejected,
        Canceled
    }
    public class CandidatJopApplication
    {
        public int Id { get; set; }
        public string AppuserId { get; set; }
        public Appuser? Appuser { get; set; }
        public int JopId { get; set; }
        public Jop? Jop { get; set; }
        public DateTime ApplicationDate { get; set; }
        public Applicationstatuse Applicationstatuse { get; set; }
        public DateTime AppliedAt { get; set; }

        public DateTime? CancelAt { get; set; }
    }
}
