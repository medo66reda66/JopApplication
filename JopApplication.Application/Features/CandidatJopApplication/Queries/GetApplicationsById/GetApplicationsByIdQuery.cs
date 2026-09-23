using JopApplication.Application.Dtos;
using MediatR;
using System.Collections.Generic;

namespace JopApplication.Features.CandidateApplications.Queries.GetApplicationsById
{
    public class GetApplicationsByIdQuery : IRequest<ShowAllApplications>
    {
        public int id { get; set; }
        public string userid { get; set; }
    }
}
