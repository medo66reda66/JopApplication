using JopApplication.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JopApplication.Features.Candidate.Commands.UpdateCandidate
{
    public class UpdateCandidateCommand : IRequest<JopApplication.Domain.Models.Candidate>
    {
        public int Id { get; set; }
        public string? GitUrl { get; set; }
        public string? LinkdenUrl { get; set; }
        public IFormFile? CVUrl { get; set; }
        public IFormFile? Profile { get; set; }
    }
}
