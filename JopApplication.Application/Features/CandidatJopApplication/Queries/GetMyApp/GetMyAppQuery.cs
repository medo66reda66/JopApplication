using JopApplication.Application.Dtos;
using MediatR;
using System.Collections.Generic;

namespace JopApplication.Features.CandidateApplications.Queries.GetMyApp
{
    public class GetMyAppQuery : IRequest<List<ShowMyApp>>
    {
        public string userid { get; set; } = string.Empty;
    }
}
