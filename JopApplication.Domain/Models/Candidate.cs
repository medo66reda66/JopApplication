using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Domain.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string AppuserId { get; set; }
        public Appuser? appuser { get; set; }
        public string CVUrl { get; set; }=string.Empty;
        public string GitUrl { get; set; } = string.Empty;
        public string LinkdenUrl {  get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;

    }
}
