using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface IJopService
    {
        Task<IEnumerable<Jop>> GetAllmYJops(string userid, CancellationToken cancellationToken);

        Task<IEnumerable<Jop>> GetAllOpenJops(CancellationToken cancellationToken);


        Task<Jop> GetJopById(int jopId, string userid, CancellationToken cancellationToken);

        Task<Jop> createJop(CreateJopRequest request, CancellationToken cancellationToken, string userid);



        Task<Jop> Edit(int jopId, UpdateJopRequest request, CancellationToken cancellationToken);


        Task Delete(int jopId, CancellationToken cancellationToken);


        Task CloseAndOpenJop(int id, CancellationToken cancellationToken);
      
    }

}


