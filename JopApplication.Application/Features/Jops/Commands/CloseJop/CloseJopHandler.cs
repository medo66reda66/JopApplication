using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Commands.CloseJop
{
    public class CloseJopHandler : IRequestHandler<CloseJopcommand>
    {
        private readonly IRepository<Jop> _jopRepository;

        public CloseJopHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task Handle(CloseJopcommand request, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                 e => e.Id == request.JopId,
                 cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            jop.jopStatus = JopStatus.Closed;

            _jopRepository.Update(jop);
            await _jopRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
