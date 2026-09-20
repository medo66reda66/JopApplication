using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Dtos
{
    public class UpdateCandidateRequest
    {
        public string? GitUrl { get; set; }
        public string? LinkdenUrl { get; set; }
        public IFormFile? CVUrl { get; set; }
        public IFormFile? Profile { get; set; }
    }
}
