using MediatR;

namespace JopApplication.Features.Candidate.Commands.DeleteCandidate
{
    public class DeleteCandidateCommand : IRequest
    {
        public int Id { get; set; }
    }
}
