using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.CandidateApplications.Commands.Create
{
    public class CreateApplicationCommand : IRequest<JopApplication.Domain.Models.CandidatJopApplication>
    {
        public string userid { get; set; } = string.Empty;
        public int jopid { get; set; }
    }
}
