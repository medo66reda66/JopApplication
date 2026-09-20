using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Dtos
{
    public class CreateCandidateRequest
    {
        [Required]
        public IFormFile CVUrl { get; set; }
        [Required]
        public string GitUrl { get; set; } = string.Empty;
        [Required]
        public string LinkdenUrl { get; set; } = string.Empty;
        [Required]
        public IFormFile Profile { get; set; } 
    }
}
