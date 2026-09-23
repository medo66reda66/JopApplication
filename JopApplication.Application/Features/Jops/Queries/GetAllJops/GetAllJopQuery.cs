using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllJop
{
    public class GetAllJopQuery : IRequest<IEnumerable<Jop>>
    {
       
        public string userid { get; set; }
    }
}
