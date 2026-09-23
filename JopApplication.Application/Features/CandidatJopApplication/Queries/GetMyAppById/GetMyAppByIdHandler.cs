using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Queries.GetMyAppById
{
    public class GetMyAppByIdHandler : IRequestHandler<GetMyAppByIdQuery, ShowMyApp>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _Candidaterepository;

        public GetMyAppByIdHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository, IRepository<JopApplication.Domain.Models.Candidate> candidaterepository)
        {
            _repository = repository;
            _Candidaterepository = candidaterepository;
        }

        public async Task<ShowMyApp> Handle(GetMyAppByIdQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _Candidaterepository.GetByIdAsync(e => e.AppuserId == request.userid && e.Id == request.id, cancellationToken: cancellationToken);
            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            var myapp = await _repository.GetByIdAsync(e => e.AppuserId == request.userid && e.Id == request.id,
                include: [e => e.Jop!],
                tracking: false,
                cancellationToken: cancellationToken);

            if (myapp == null)
            {
                throw new Exception("Application not found");
            }

            var show = new ShowMyApp
            {
                CVUrl = candidate.CVUrl,
                GitUrl = candidate.GitUrl,
                LinkdenUrl = candidate.LinkdenUrl,
                Profile = candidate.Profile,
                Title = myapp.Jop!.Title,
                Description = myapp.Jop.Description,
                Location = myapp.Jop.Location,
                PostedDate = myapp.Jop.PostedDate,
                jopStatus = myapp.Jop.jopStatus,
                ImageUrl = myapp.Jop.ImageUrl
            };

            return show;
        }
    }
}
