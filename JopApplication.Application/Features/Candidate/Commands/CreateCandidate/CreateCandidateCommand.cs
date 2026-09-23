using JopApplication.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JopApplication.Features.Candidate.Commands.CreateCandidate
{
    public class CreateCandidateCommand : IRequest<JopApplication.Domain.Models.Candidate>
    {
        [Required]
        public IFormFile CVUrl { get; set; } = null!;
        [Required]
        public string GitUrl { get; set; } = string.Empty;
        [Required]
        public string LinkdenUrl { get; set; } = string.Empty;
        [Required]
        public IFormFile Profile { get; set; } = null!;

        public string userid { get; set; } = string.Empty;
    }
}
