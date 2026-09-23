using JopApplication.Application.Interfaces;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.Candidate.Commands.CreateCandidate
{
    public class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, JopApplication.Domain.Models.Candidate>
    {
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _candidateRepository;

        public CreateCandidateHandler(IRepository<JopApplication.Domain.Models.Candidate> candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<JopApplication.Domain.Models.Candidate> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            var can =await _candidateRepository.GetByIdAsync(e => e.AppuserId == request.userid, cancellationToken: cancellationToken);
            if(can != null)
            {
                return null;
            }
            var candidate = new JopApplication.Domain.Models.Candidate
            {
                AppuserId = request.userid,
                GitUrl = request.GitUrl,
                LinkdenUrl = request.LinkdenUrl,
            };

            if(request.CVUrl != null && request.CVUrl.Length > 0 )
            {
                var filenamecv = Guid.NewGuid().ToString() + Path.GetExtension(request.CVUrl.FileName);
                var filepathcv = Path.Combine("wwwroot", "Cv", filenamecv);
                using (var stream = new FileStream(filepathcv, FileMode.Create))
                {
                    await request.CVUrl.CopyToAsync(stream, cancellationToken);
                }
                candidate.CVUrl = filepathcv;
            }

            if(request.Profile != null && request.Profile.Length > 0 )
            {
                var filenameprofile = Guid.NewGuid().ToString() + Path.GetExtension(request.Profile.FileName);
                var filepathprofile = Path.Combine("wwwroot", "Cv", filenameprofile);
                using (var stream = new FileStream(filepathprofile, FileMode.Create))
                {
                    await request.Profile.CopyToAsync(stream, cancellationToken);
                }
                candidate.Profile = filepathprofile;
            }

            await _candidateRepository.AddAsync(candidate, cancellationToken);
            await _candidateRepository.SaveChangesAsync(cancellationToken);

            return candidate;
        }
    }
}
