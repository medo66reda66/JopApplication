using JopApplication.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JopApplication.Features.Jops.Commands.CreateJop
{
    public class CreateJopCommand :IRequest<Jop>
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        [Required]
        public IFormFile? ImageUrl { get; set; }

        public string userid { get; set; }
    }
}
