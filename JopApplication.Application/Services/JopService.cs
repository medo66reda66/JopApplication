using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Services
{
    public class JopService : IJopService
    {
        private readonly IRepository<Jop> _jopRepository;

        public JopService(IRepository<Jop> jopRepository)
        {
            _jopRepository = jopRepository;
        }

        
        public async Task<IEnumerable<Jop>> GetAllmYJops(string userid , CancellationToken cancellationToken)
        {
            return await _jopRepository.GetAllAsync(e=>e.AppuserId == userid , cancellationToken: cancellationToken);
        }

        // Get All Open Jobs (Public - for Candidates landing page)
        public async Task<IEnumerable<Jop>> GetAllOpenJops(CancellationToken cancellationToken)
        {
            return await _jopRepository.GetAllAsync(
                e => e.jopStatus == JopStatus.Open,
                cancellationToken: cancellationToken);
        }

        public async Task<Jop> GetJopById(int jopId,string userid, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                e => e.Id == jopId && e.AppuserId == userid,
                cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            return jop;
        }

        
        public async Task<Jop> createJop(CreateJopRequest request, CancellationToken cancellationToken, string userid)
        {
            var jop = new Jop
            {
                Title       = request.Title,
                Description = request.Description,
                Location    = request.Location,
                PostedDate  = DateTime.UtcNow,
                jopStatus   = JopStatus.Open,
                AppuserId   = userid,
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

        
        public async Task<Jop> Edit(int jopId, UpdateJopRequest request, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                e => e.Id == jopId,
                cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            jop.Title       = request.Title;
            jop.Description = request.Description;
            jop.Location    = request.Location;
            jop.jopStatus   = request.jopStatus;

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

       
        public async Task Delete(int jopId, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                e => e.Id == jopId,
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

        
        public async Task CloseAndOpenJop(int id, CancellationToken cancellationToken)
        {
            var jop = await _jopRepository.GetByIdAsync(
                e => e.Id == id,
                cancellationToken: cancellationToken);

            if (jop == null)
                throw new Exception("Job not found");

            jop.jopStatus = jop.jopStatus == JopStatus.Open
                ? JopStatus.Closed
                : JopStatus.Open;

            _jopRepository.Update(jop);
            await _jopRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
