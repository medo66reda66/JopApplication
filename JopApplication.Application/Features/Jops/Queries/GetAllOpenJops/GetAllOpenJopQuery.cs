using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllOpenJops
{
    public class GetAllOpenJopQuery : IRequest<IEnumerable<Jop>>
    {
        public string userid { get; set; }
    }
}
