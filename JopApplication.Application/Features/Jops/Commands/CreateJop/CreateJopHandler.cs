using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Commands.CreateJop
{
    public class CreateJopHandler : IRequestHandler<CreateJopCommand, Jop>
    {
        private readonly IRepository<Jop> _jopRepository;

        public CreateJopHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task<Jop> Handle(CreateJopCommand request, CancellationToken cancellationToken)
        {
            var jop = new Jop
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                PostedDate = DateTime.UtcNow,
                jopStatus = JopStatus.Open,
                AppuserId = request.userid,
            };

            if (request.ImageUrl != null && request.ImageUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageUrl.FileName);
                var filePath = Path.Combine("wwwroot", "Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageUrl.CopyToAsync(stream, cancellationToken);
                }

                jop.ImageUrl = filePath;
            }

            await _jopRepository.AddAsync(jop, cancellationToken);
            await _jopRepository.SaveChangesAsync(cancellationToken);

            return jop;
        }
    }
}
