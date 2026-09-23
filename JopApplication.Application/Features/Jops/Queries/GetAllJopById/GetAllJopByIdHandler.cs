using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Queries.GetAllJopById
{
    public class GetAllJopByIdHandler : IRequestHandler<GetAllJopByIdQuery, Jop>
    {
        private readonly IRepository<Jop> _jopRepository;

        public GetAllJopByIdHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task<Jop> Handle(GetAllJopByIdQuery request, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                 e => e.Id == request.Id && e.AppuserId == request.userid,
                 cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            return jop;
        }
    }
}
