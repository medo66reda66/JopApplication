using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Commands.Cancel
{
    public class CancelApplicationHandler : IRequestHandler<CancelApplicationCommand>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;

        public CancelApplicationHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
        {
            var app = await _repository.GetByIdAsync(
                e => e.Id == request.Appid && e.Jop.AppuserId == request.userid,
                cancellationToken: cancellationToken
            );

            if (app == null)
            {
                throw new Exception("App not found");
            }

            if(app.Applicationstatuse == Applicationstatuse.Interview || app.Applicationstatuse == Applicationstatuse.Accepted)
            {
                throw new Exception("No Cancel");
            }

            app.Applicationstatuse = Applicationstatuse.Canceled;
            app.CancelAt = DateTime.Now;
            
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
