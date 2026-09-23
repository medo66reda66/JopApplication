using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.CandidateApplications.Commands.UpdateStatus
{
    public class UpdateStatusCommand : IRequest
    {
        public int appId { get; set; }
        public Applicationstatuse status { get; set; }

        public string userid { get; set; }
    }
}
