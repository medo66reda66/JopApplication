using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface ICandidateService
    {
        Task<Candidate> Create(CreateCandidateRequest createCandidate, string userid, CancellationToken cancellationToken);


        //Update
         Task<Candidate> Update(
            int candidateId,
            UpdateCandidateRequest request,
            CancellationToken cancellationToken);


         Task Delete(
            int candidateId,
            CancellationToken cancellationToken);
     
    }
}
