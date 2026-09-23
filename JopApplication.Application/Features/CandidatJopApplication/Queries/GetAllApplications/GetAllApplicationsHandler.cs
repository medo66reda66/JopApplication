using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Queries.GetAllApplications
{
    public class GetAllApplicationsHandler : IRequestHandler<GetAllApplicationsQuery, List<ShowAllApplications>>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _Candidaterepository;

        public GetAllApplicationsHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository, IRepository<JopApplication.Domain.Models.Candidate> candidaterepository)
        {
            _repository = repository;
            _Candidaterepository = candidaterepository;
        }

        public async Task<List<ShowAllApplications>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _repository.GetAllAsync(
                e => e.Applicationstatuse != Applicationstatuse.Canceled && e.Jop.AppuserId == request.userid,
                include: [e => e.Jop!, e => e.Appuser!],
                tracking: false,
                cancellationToken: cancellationToken);

            var show = new List<ShowAllApplications>();

            foreach (var application in applications)
            {
                var candidate = await _Candidaterepository.GetByIdAsync(
                    e => e.AppuserId == application.AppuserId,
                    cancellationToken: cancellationToken);

                if (candidate == null)
                {
                    continue;
                }

                show.Add(new ShowAllApplications
                {
                    ApplicationId = application.Id,
                    ApplicationDate = application.ApplicationDate,
                    AppliedAt = application.AppliedAt,
                    Applicationstatuse = application.Applicationstatuse,
                    CVUrl = candidate.CVUrl,
                    GitUrl = candidate.GitUrl,
                    LinkdenUrl = candidate.LinkdenUrl,
                    Profile = candidate.Profile,
                    FirstName = application.Appuser!.FirstName,
                    LastName = application.Appuser.LastName,
                    UserName = application.Appuser.UserName!,
                    Email = application.Appuser.Email!,
                    Phone = application.Appuser.PhoneNumber!,
                    Address = application.Appuser.Address,
                    City = application.Appuser.City,
                    Country = application.Appuser.Country,
                    Title = application.Jop!.Title,
                    Description = application.Jop.Description,
                    Location = application.Jop.Location,
                    PostedDate = application.Jop.PostedDate,
                    JopStatus = application.Jop.jopStatus,
                    ImageUrl = application.Jop.ImageUrl
                });
            }

            return show;
        }
    }
}
