using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllJopById
{
    public class GetAllJopByIdQuery : IRequest<Jop>
    {
        public int Id { get; set; }
        public string userid { get; set; }
    }
}
