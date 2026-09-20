using JopApplication.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Dtos
{
    public class UpdateJopRequest
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        [Required]
        public JopStatus jopStatus { get; set; } 
        [Required]
        public IFormFile? ImageUrl { get; set; }
    }
}
