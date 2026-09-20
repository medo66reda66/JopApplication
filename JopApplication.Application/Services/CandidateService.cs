using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IRepository<Candidate> _candidateRepository;

        public CandidateService(IRepository<Candidate> candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        //Create
        public async Task<Candidate> Create(CreateCandidateRequest createCandidate,string userid , CancellationToken cancellationToken)
        {
            var Candidate = new Candidate
            {
                AppuserId = userid,
                GitUrl = createCandidate.GitUrl,
                LinkdenUrl = createCandidate.LinkdenUrl,
            };
            if(createCandidate.CVUrl != null && createCandidate.CVUrl.Length > 0 )
            {
                var filenamecv = Guid.NewGuid().ToString() + Path.GetExtension(createCandidate.CVUrl.FileName);
                var filepathcv = Path.Combine("wwwroot", "Cv", filenamecv);
                using (var stream = new FileStream(filepathcv, FileMode.Create))
                {
                    await createCandidate.CVUrl.CopyToAsync(stream,cancellationToken);
                }
                Candidate.CVUrl = filepathcv;
            }
            if(createCandidate.Profile != null && createCandidate.Profile.Length > 0 )
            {
                var filenameprofile = Guid.NewGuid().ToString() + Path.GetExtension(createCandidate.Profile.FileName);
                var filepathprofile = Path.Combine("wwwroot", "Cv", filenameprofile);
                using (var stream = new FileStream(filepathprofile, FileMode.Create))
                {
                    await createCandidate.Profile.CopyToAsync(stream,cancellationToken);
                }
                Candidate.Profile = filepathprofile;
            }
            await _candidateRepository.AddAsync(Candidate, cancellationToken);
            await _candidateRepository.SaveChangesAsync(cancellationToken);

            return Candidate;
        }

        //Update
        public async Task<Candidate> Update(
            int candidateId,
            UpdateCandidateRequest request,
            CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(
                e => e.Id == candidateId,
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

                var filenamecv =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(request.CVUrl.FileName);

                var filepathcv = Path.Combine(
                    "wwwroot",
                    "Cv",
                    filenamecv);

                using (var stream = new FileStream(filepathcv, FileMode.Create))
                {
                    await request.CVUrl.CopyToAsync(
                        stream,
                        cancellationToken);
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

                var filenameprofile =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(request.Profile.FileName);

                var filepathprofile = Path.Combine(
                    "wwwroot",
                    "Profile",
                    filenameprofile);

                using (var stream = new FileStream(filepathprofile, FileMode.Create))
                {
                    await request.Profile.CopyToAsync(
                        stream,
                        cancellationToken);
                }

                candidate.Profile = filepathprofile;
            }

            _candidateRepository.Update(candidate);

            await _candidateRepository.SaveChangesAsync(cancellationToken);

            return candidate;
        }

        //Delete
        public async Task Delete(
            int candidateId,
            CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(
                e => e.Id == candidateId,
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
