using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllJop
{
    public class GetAllJopHandler : IRequestHandler<GetAllJopQuery, IEnumerable<Jop>>
    {
        private readonly IRepository<Jop> _jopRepository;

        public GetAllJopHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task<IEnumerable<Jop>> Handle(GetAllJopQuery request, CancellationToken cancellationToken)
        {
            return await _jopRepository.GetAllAsync(e => e.AppuserId == request.userid, cancellationToken: cancellationToken);
        }
    }
}
