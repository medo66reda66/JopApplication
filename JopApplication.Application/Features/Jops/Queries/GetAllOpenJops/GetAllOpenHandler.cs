using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllOpenJops
{
    public class GetAllOpenHandler : IRequestHandler<GetAllOpenJopQuery, IEnumerable<Jop>>
    {
        private readonly IRepository<Jop> _jopRepository;

        public GetAllOpenHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task<IEnumerable<Jop>> Handle(GetAllOpenJopQuery request, CancellationToken cancellationToken)
        {
            return await _jopRepository.GetAllAsync(
                e => e.jopStatus == JopStatus.Open,
                cancellationToken: cancellationToken);
        }
    }
}
