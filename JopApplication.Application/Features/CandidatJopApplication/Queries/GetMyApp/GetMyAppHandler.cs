using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Queries.GetMyApp
{
    public class GetMyAppHandler : IRequestHandler<GetMyAppQuery, List<ShowMyApp>>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _Candidaterepository;

        public GetMyAppHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository, IRepository<JopApplication.Domain.Models.Candidate> candidaterepository)
        {
            _repository = repository;
            _Candidaterepository = candidaterepository;
        }

        public async Task<List<ShowMyApp>> Handle(GetMyAppQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _Candidaterepository.GetByIdAsync(e => e.AppuserId == request.userid, cancellationToken: cancellationToken);
            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            var myapp = await _repository.GetAllAsync(e => e.AppuserId == request.userid,
                include: [e => e.Jop!],
                tracking: false,
                cancellationToken: cancellationToken);

            var show = myapp.Select(e => new ShowMyApp
            {
                CVUrl = candidate.CVUrl,
                GitUrl = candidate.GitUrl,
                LinkdenUrl = candidate.LinkdenUrl,
                Profile = candidate.Profile,
                Title = e.Jop!.Title,
                Description = e.Jop.Description,
                Location = e.Jop.Location,
                PostedDate = e.Jop.PostedDate,
                jopStatus = e.Jop.jopStatus,
                ImageUrl = e.Jop.ImageUrl
            }).ToList();

            return show;
        }
    }
}
