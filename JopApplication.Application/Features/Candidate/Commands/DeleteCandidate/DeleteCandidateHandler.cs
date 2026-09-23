using JopApplication.Application.Interfaces;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.Candidate.Commands.DeleteCandidate
{
    public class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand>
    {
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _candidateRepository;

        public DeleteCandidateHandler(IRepository<JopApplication.Domain.Models.Candidate> candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(
                e => e.Id == request.Id,
                cancellationToken: cancellationToken);

            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            // Delete CV file
            if (!string.IsNullOrEmpty(candidate.CVUrl))
            {
                if (File.Exists(candidate.CVUrl))
                {
                    File.Delete(candidate.CVUrl);
                }
            }

            // Delete Profile file
            if (!string.IsNullOrEmpty(candidate.Profile))
            {
                if (File.Exists(candidate.Profile))
                {
                    File.Delete(candidate.Profile);
                }
            }

            _candidateRepository.Delete(candidate);
            await _candidateRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
