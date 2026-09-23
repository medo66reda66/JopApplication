using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;

namespace JopApplication.Features.Jops.Commands.UpdateJop
{
    public class UpdateJopHandler : IRequestHandler<UpdateJopCommand, Jop>
    {
        private readonly IRepository<Jop> _jopRepository;

        public UpdateJopHandler(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        public async Task<Jop> Handle(UpdateJopCommand request, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
               e => e.Id == request.jopId,
               cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            jop.Title = request.Title;
            jop.Description = request.Description;
            jop.Location = request.Location;


            if (request.ImageUrl != null && request.ImageUrl.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(jop.ImageUrl) && File.Exists(jop.ImageUrl))
                {
                    File.Delete(jop.ImageUrl);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageUrl.FileName);
                var filePath = Path.Combine("wwwroot", "Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageUrl.CopyToAsync(stream, cancellationToken);
                }

                jop.ImageUrl = filePath;
            }

            _jopRepository.Update(jop);
            await _jopRepository.SaveChangesAsync(cancellationToken);

            return jop;
        }
    }
}
