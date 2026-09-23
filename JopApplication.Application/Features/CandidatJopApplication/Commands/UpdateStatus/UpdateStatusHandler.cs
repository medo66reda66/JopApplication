using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Features.CandidateApplications.Commands.UpdateStatus
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand>
    {
        private readonly IRepository<JopApplication.Domain.Models.CandidatJopApplication> _repository;

        public UpdateStatusHandler(IRepository<JopApplication.Domain.Models.CandidatJopApplication> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var app = await _repository.GetByIdAsync(
                e => e.Id == request.appId && e.Jop.AppuserId == request.userid,
                cancellationToken: cancellationToken);

            if (app == null)
            {
                throw new Exception("Application not found");
            }

            switch (app.Applicationstatuse)
            {
                case Applicationstatuse.Applied:
                    if (request.status != Applicationstatuse.UnderReview)
                        throw new Exception("Not Allowed");
                    break;
                case Applicationstatuse.UnderReview:
                    if (request.status != Applicationstatuse.Interview)
                        throw new Exception("Not Allowed");
                    break;
                case Applicationstatuse.Interview:
                    if (request.status != Applicationstatuse.Accepted &&
                        request.status != Applicationstatuse.Rejected &&
                        request.status != Applicationstatuse.Canceled)
                    {
                        throw new Exception("Not Allowed");
                    }
                    break;
                case Applicationstatuse.Accepted:
                case Applicationstatuse.Rejected:
                case Applicationstatuse.Canceled:
                    throw new Exception("Application status cannot be changed.");
                default:
                    throw new Exception("Invalid application status.");
            }

            app.Applicationstatuse = request.status;
            if(app.Applicationstatuse == Applicationstatuse.Canceled)
            {
                app.CancelAt = DateTime.Now;
            }

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
