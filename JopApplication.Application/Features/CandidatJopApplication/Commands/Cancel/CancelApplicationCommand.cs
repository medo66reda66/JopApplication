using MediatR;

namespace JopApplication.Features.CandidateApplications.Commands.Cancel
{
    public class CancelApplicationCommand : IRequest
    {
        public int Appid { get; set; }
        public string userid { get; set; }
    }
}
