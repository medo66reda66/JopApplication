using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Commands.Create
{
    public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommand, CandidatJopApplication>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _candidateRepository;

        public CreateApplicationHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository, IRepository<JopApplication.Domain.Models.Candidate> candidateRepository)
        {
            _repository = repository;
            _candidateRepository = candidateRepository;
        }

        public async Task<JopApplication.Domain.Models.CandidatJopApplication> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var apps = await _repository.GetByIdAsync(e => e.JopId == request.jopid && e.AppuserId == request.userid, cancellationToken: cancellationToken);
            var candidate = await _candidateRepository.GetByIdAsync(e => e.AppuserId == request.userid, cancellationToken: cancellationToken);
            
            if (candidate == null)
            {
                throw new Exception("You must create a candidate profile before applying for a job.");
            }

            if (apps != null)
            {
                throw new Exception("No duplicate application.");
            }
            
            var app = new CandidatJopApplication
            {
                AppuserId = request.userid,
                JopId = request.jopid,
                ApplicationDate = DateTime.Now,
                Applicationstatuse = Applicationstatuse.Applied,
                AppliedAt = DateTime.Now,
            };
            
            await _repository.AddAsync(app, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return app;
        }
    }
}
