using JopApplication.Application.Dtos;
using MediatR;
using System.Collections.Generic;

namespace JopApplication.Features.CandidateApplications.Queries.GetAllApplications
{
    public class GetAllApplicationsQuery : IRequest<List<ShowAllApplications>>
    {
        public string userid { get; set; }
    }
}
