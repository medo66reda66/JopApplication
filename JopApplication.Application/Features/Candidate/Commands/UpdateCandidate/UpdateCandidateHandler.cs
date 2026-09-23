using JopApplication.Application.Interfaces;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.Candidate.Commands.UpdateCandidate
{
    public class UpdateCandidateHandler : IRequestHandler<UpdateCandidateCommand, JopApplication.Domain.Models.Candidate>
    {
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _candidateRepository;

        public UpdateCandidateHandler(IRepository<JopApplication.Domain.Models.Candidate> candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<JopApplication.Domain.Models.Candidate> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(
                e => e.Id == request.Id,
                cancellationToken: cancellationToken);

            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            candidate.GitUrl = request.GitUrl;
            candidate.LinkdenUrl = request.LinkdenUrl;

            // Update CV
            if (request.CVUrl != null && request.CVUrl.Length > 0)
            {
                if (!string.IsNullOrEmpty(candidate.CVUrl))
                {
                    if (File.Exists(candidate.CVUrl))
                    {
                        File.Delete(candidate.CVUrl);
                    }
                }

                var filenamecv = Guid.NewGuid().ToString() + Path.GetExtension(request.CVUrl.FileName);
                var filepathcv = Path.Combine("wwwroot", "Cv", filenamecv);

                using (var stream = new FileStream(filepathcv, FileMode.Create))
                {
                    await request.CVUrl.CopyToAsync(stream, cancellationToken);
                }

                candidate.CVUrl = filepathcv;
            }

            if (request.Profile != null && request.Profile.Length > 0)
            {
                if (!string.IsNullOrEmpty(candidate.Profile))
                {
                    if (File.Exists(candidate.Profile))
                    {
                        File.Delete(candidate.Profile);
                    }
                }

                var filenameprofile = Guid.NewGuid().ToString() + Path.GetExtension(request.Profile.FileName);
                var filepathprofile = Path.Combine("wwwroot", "Profile", filenameprofile);

                using (var stream = new FileStream(filepathprofile, FileMode.Create))
                {
                    await request.Profile.CopyToAsync(stream, cancellationToken);
                }

                candidate.Profile = filepathprofile;
            }

            _candidateRepository.Update(candidate);
            await _candidateRepository.SaveChangesAsync(cancellationToken);

            return candidate;
        }
    }
}
