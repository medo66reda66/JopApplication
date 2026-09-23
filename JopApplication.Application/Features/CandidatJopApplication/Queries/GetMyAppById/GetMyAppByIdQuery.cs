using JopApplication.Application.Dtos;
using MediatR;

namespace JopApplication.Features.CandidateApplications.Queries.GetMyAppById
{
    public class GetMyAppByIdQuery : IRequest<ShowMyApp>
    {
        public int id { get; set; }
        public string userid { get; set; } = string.Empty;
    }
}
