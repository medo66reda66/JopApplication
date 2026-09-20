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
    public class CandidateJopAppService : ICandidateJopAppService
    {
        private readonly IRepository<CandidatJopApplication> _repository;
        private readonly IRepository<Candidate> _Candidaterepository;

        public CandidateJopAppService(IRepository<CandidatJopApplication> repository, IRepository<Candidate> candidaterepository)
        {
            _repository = repository;
            _Candidaterepository = candidaterepository;
        }

        public async Task<List<ShowAllApplications>> GetAllApplications(
             CancellationToken cancellationToken)
        {
            var applications = await _repository.GetAllAsync(
                e => e.Applicationstatuse != Applicationstatuse.Canceled,
                include: [e => e.Jop!, e => e.Appuser!],
                tracking: false,
                cancellationToken: cancellationToken);

            var show = new List<ShowAllApplications>();

            foreach (var application in applications)
            {
                var candidate = await _Candidaterepository.GetByIdAsync(
                    e => e.AppuserId == application.AppuserId,
                    cancellationToken: cancellationToken);

                if (candidate == null)
                {
                    continue;
                }

                show.Add(new ShowAllApplications
                {
                    // Application
                    ApplicationId = application.Id,
                    ApplicationDate = application.ApplicationDate,
                    AppliedAt = application.AppliedAt,
                    Applicationstatuse = application.Applicationstatuse,

                    // Candidate
                    CVUrl = candidate.CVUrl,
                    GitUrl = candidate.GitUrl,
                    LinkdenUrl = candidate.LinkdenUrl,
                    Profile = candidate.Profile,

                    // User
                    FirstName = application.Appuser!.FirstName,
                    LastName = application.Appuser.LastName,
                    UserName = application.Appuser.UserName!,
                    Email = application.Appuser.Email!,
                    Phone = application.Appuser.PhoneNumber!,
                    Address = application.Appuser.Address,
                    City = application.Appuser.City,
                    Country = application.Appuser.Country,

                    // Job
                    Title = application.Jop!.Title,
                    Description = application.Jop.Description,
                    Location = application.Jop.Location,
                    PostedDate = application.Jop.PostedDate,
                    JopStatus = application.Jop.jopStatus,
                    ImageUrl = application.Jop.ImageUrl
                });
            }

            return show;
        }

        public async Task<List<ShowMyApp>> MyApp(string userid , CancellationToken cancellationToken)
        {
            var candidate = await _Candidaterepository.GetByIdAsync(e => e.AppuserId == userid, cancellationToken: cancellationToken);
            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            var myapp =await _repository.GetAllAsync(e => e.AppuserId == userid,
                include: [e => e.Jop!],
                tracking: false,
                cancellationToken: cancellationToken);

            var show = myapp.Select(e => new ShowMyApp
            {
                CVUrl = candidate.CVUrl,
                GitUrl = candidate.GitUrl,
                LinkdenUrl = candidate.LinkdenUrl,
                Profile = candidate.Profile,
                Title = e.Jop!.Title,
                Description = e.Jop.Description,
                Location = e.Jop.Location,
                PostedDate = e.Jop.PostedDate,
                jopStatus = e.Jop.jopStatus,
                ImageUrl = e.Jop.ImageUrl
            }).ToList();

            return show;
        }
        public async Task<ShowMyApp> MyAppById(int id , string userid , CancellationToken cancellationToken)
        {
            var candidate = await _Candidaterepository.GetByIdAsync(e => e.AppuserId == userid, cancellationToken: cancellationToken);
            if (candidate == null)
            {
                throw new Exception("Candidate not found");
            }

            var myapp =await _repository.GetByIdAsync(e => e.AppuserId == userid && e.Id == id,
                include: [e => e.Jop!],
                tracking: false,
                cancellationToken: cancellationToken);

            var show = new ShowMyApp
            {
                CVUrl = candidate.CVUrl,
                GitUrl = candidate.GitUrl,
                LinkdenUrl = candidate.LinkdenUrl,
                Profile = candidate.Profile,
                Title = myapp.Jop!.Title,
                Description = myapp.Jop.Description,
                Location = myapp.Jop.Location,
                PostedDate = myapp.Jop.PostedDate,
                jopStatus = myapp.Jop.jopStatus,
                ImageUrl = myapp.Jop.ImageUrl
            };

            return show;
        }
        public async Task<CandidatJopApplication> Create(string userid , int jopid,CancellationToken cancellationToken)
        {
            var Apps =await _repository.GetByIdAsync(e=>e.JopId == jopid && e.AppuserId == userid,cancellationToken:cancellationToken);
            var candidate =await _Candidaterepository.GetByIdAsync(e=>e.AppuserId == userid,cancellationToken:cancellationToken);
            if (candidate == null)
            {
                throw new Exception("You must create a candidate profile before applying for a job.");
            }

            if (Apps != null)
            {
                throw new Exception("No duplicate application.");
            }
            var App = new CandidatJopApplication
            {
                AppuserId = userid,
                JopId = jopid,
                ApplicationDate = DateTime.Now,
                Applicationstatuse = Applicationstatuse.Applied,
                AppliedAt = DateTime.Now,
            };
           await _repository.AddAsync(App , cancellationToken);
           await _repository.SaveChangesAsync(cancellationToken);

            return App;
        }
        public async Task UpdateStatus(
                int appId,
                Applicationstatuse status,
                CancellationToken cancellationToken)
        {
            var app = await _repository.GetByIdAsync(
                e => e.Id == appId,
                cancellationToken: cancellationToken);

            if (app == null)
            {
                throw new Exception("Application not found");
            }

            switch (app.Applicationstatuse)
            {
                case Applicationstatuse.Applied:

                    if (status != Applicationstatuse.UnderReview)
                        throw new Exception("Not Allowed");

                    break;

                case Applicationstatuse.UnderReview:

                    if (status != Applicationstatuse.Interview)
                        throw new Exception("Not Allowed");

                    break;

                case Applicationstatuse.Interview:

                    if (status != Applicationstatuse.Accepted &&
                        status != Applicationstatuse.Rejected &&
                        status != Applicationstatuse.Canceled)
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

            app.Applicationstatuse = status;
            if(app.Applicationstatuse == Applicationstatuse.Canceled)
            {
                app.CancelAt = DateTime.Now;
            }

            await _repository.SaveChangesAsync(cancellationToken);
        }
        public async Task updateCanceled(int Appid ,CancellationToken cancellationToken)
        {
            var app =await _repository.GetByIdAsync(
                e=>e.Id == Appid,
                cancellationToken:cancellationToken
                );

            if(app.Applicationstatuse == Applicationstatuse.Interview || app.Applicationstatuse == Applicationstatuse.Accepted)
            {
                throw new Exception("No Cancel");
            }

            if (app == null )
            {
                throw new Exception("App not found");
            }
            app.Applicationstatuse = Applicationstatuse.Canceled;
            app.CancelAt = DateTime.Now;
            await _repository.SaveChangesAsync(cancellationToken);
        }

    }
}
