using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Queries.GetApplicationsById
{
    public class GetApplicationsByIdHandler : IRequestHandler<GetApplicationsByIdQuery,ShowAllApplications>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;
        private readonly IRepository<JopApplication.Domain.Models.Candidate> _Candidaterepository;

        public GetApplicationsByIdHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository, IRepository<JopApplication.Domain.Models.Candidate> candidaterepository)
        {
            _repository = repository;
            _Candidaterepository = candidaterepository;
        }

        public async Task<ShowAllApplications> Handle(GetApplicationsByIdQuery request, CancellationToken cancellationToken)
        {
                var applications = await _repository.GetByIdAsync(
                e => e.Applicationstatuse != Applicationstatuse.Canceled 
                && e.Jop.AppuserId == request.userid && e.Id == request.id,
                include: [e => e.Jop!, e => e.Appuser!],
                tracking: false,
                cancellationToken: cancellationToken);

                var candidate = await _Candidaterepository.GetByIdAsync(
                    e => e.AppuserId == applications.AppuserId,
                    cancellationToken: cancellationToken);

                if (candidate == null)
                {
                return null;
                }

                var show = new ShowAllApplications
                {
                        ApplicationId = applications.Id,
                        ApplicationDate = applications.ApplicationDate,
                        AppliedAt = applications.AppliedAt,
                        Applicationstatuse = applications.Applicationstatuse,
                        CVUrl = candidate.CVUrl,
                        GitUrl = candidate.GitUrl,
                        LinkdenUrl = candidate.LinkdenUrl,
                        Profile = candidate.Profile,
                        FirstName = applications.Appuser!.FirstName,
                        LastName = applications.Appuser.LastName,
                        UserName = applications.Appuser.UserName!,
                        Email = applications.Appuser.Email!,
                        Phone = applications.Appuser.PhoneNumber!,
                        Address = applications.Appuser.Address,
                        City = applications.Appuser.City,
                        Country = applications.Appuser.Country,
                        Title = applications.Jop!.Title,
                        Description = applications.Jop.Description,
                        Location = applications.Jop.Location,
                        PostedDate = applications.Jop.PostedDate,
                        JopStatus = applications.Jop.jopStatus,
                        ImageUrl = applications.Jop.ImageUrl
                };
            return show;
        }

            
    }
}
