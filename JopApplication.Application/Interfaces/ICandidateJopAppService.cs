using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface ICandidateJopAppService
    {

        Task<List<ShowAllApplications>> GetAllApplications(
         CancellationToken cancellationToken);

        Task<List<ShowMyApp>> MyApp(string userid, CancellationToken cancellationToken);
        Task<ShowMyApp> MyAppById(int id, string userid, CancellationToken cancellationToken);

        Task<CandidatJopApplication> Create(string userid, int jopid, CancellationToken cancellationToken);
       
        Task UpdateStatus(
              int appId,
              Applicationstatuse status,
              CancellationToken cancellationToken);

        Task updateCanceled(int Appid, CancellationToken cancellationToken);


    }
}
