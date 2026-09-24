using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Features.Candidate.Commands.CreateCandidate;
using JopApplication.Features.Candidate.Commands.UpdateCandidate;
using JopApplication.Features.Candidate.Commands.DeleteCandidate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DS.CANDIDATE_ROLE)]
    public class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new candidate profile for the authenticated user.
        /// </summary>
        /// <param name="request">The candidate information including CV, GitHub, LinkedIn, and profile details.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>The newly created candidate profile.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromQuery] CreateCandidateRequest request,
            CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userid == null)
            {
                return NotFound("user not found");
            }

            var command = new CreateCandidateCommand
            {
                CVUrl = request.CVUrl,
                GitUrl = request.GitUrl,
                LinkdenUrl = request.LinkdenUrl,
                Profile = request.Profile,
                userid = userid
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (request == null)
            {
                return BadRequest("No Create MENY Candidate");
            }

            return Ok(result);
        }


        /// <summary>
        /// Updates an existing candidate profile.
        /// </summary>
        /// <param name="id">The unique identifier of the candidate profile.</param>
        /// <param name="request">The updated candidate information including CV, GitHub, LinkedIn, and profile details.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>The updated candidate profile.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromQuery] UpdateCandidateRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCandidateCommand
            {
                Id = id,
                GitUrl = request.GitUrl,
                LinkdenUrl = request.LinkdenUrl,
                CVUrl = request.CVUrl,
                Profile = request.Profile,
            };

            var result = await _mediator.Send(command, cancellationToken);
            if (request == null)
            {
                return BadRequest();                                     
            }

            return Ok(result);
        }


        /// <summary>
        /// Deletes an existing candidate profile.
        /// </summary>
        /// <param name="id">The unique identifier of the candidate profile to delete.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>No content when the candidate profile is successfully deleted.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteCandidateCommand { Id = id },
                cancellationToken);

            return NoContent();
        }

    }

}
