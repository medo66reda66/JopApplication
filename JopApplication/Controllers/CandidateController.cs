using Azure.Core;
using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DS.CANDIDATE_ROLE)]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }



        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] CreateCandidateRequest request, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userid == null)
            {
                return NotFound("user not found");
            }
            var result = await _candidateService.Create(request, userid,cancellationToken);
            if(request == null)
            {
                return BadRequest("invalid create candidate");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCandidateRequest request , CancellationToken cancellationToken)
        {
            var result = await _candidateService.Update(id,request,cancellationToken);
            if (request == null)
            {
                return BadRequest("invalid create candidate");
            }
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
              int id,
              CancellationToken cancellationToken)
        {
            await _candidateService.Delete(id, cancellationToken);

            return NoContent();
        }

    }
}
