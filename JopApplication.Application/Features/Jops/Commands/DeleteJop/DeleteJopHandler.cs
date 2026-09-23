using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Commands.DeleteJop
{
    public class DeleteJopHandler : IRequestHandler<DeleteJopCommand>
    {
        private readonly IRepository<Jop> _jopRepository;

        public DeleteJopHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task Handle(DeleteJopCommand request, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                e => e.Id == request.JopId,
                cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            // Delete image file if exists
            if (!string.IsNullOrEmpty(jop.ImageUrl) && File.Exists(jop.ImageUrl))
            {
                File.Delete(jop.ImageUrl);
            }

            _jopRepository.Delete(jop);
            await _jopRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
