using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using JopApplication.Features.CandidateApplications.Commands.Cancel;
using JopApplication.Features.CandidateApplications.Commands.Create;
using JopApplication.Features.CandidateApplications.Commands.UpdateStatus;
using JopApplication.Features.CandidateApplications.Queries.GetAllApplications;
using JopApplication.Features.CandidateApplications.Queries.GetApplicationsById;
using JopApplication.Features.CandidateApplications.Queries.GetMyApp;
using JopApplication.Features.CandidateApplications.Queries.GetMyAppById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppController(IMediator mediator)
        {
            _mediator = mediator;
        }

    /// <summary>
    /// Retrieves all job applications submitted to jobs owned by the authenticated recruiter.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
    /// <returns>A list of all applications associated with the recruiter's jobs.</returns>
    [HttpGet("GetAllApplications")]
    [Authorize(Roles = DS.RECRUITER_ROLE)]
    public async Task<ActionResult<List<ShowAllApplications>>> GetAllApplications(
          CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var applications = await _mediator.Send(new GetAllApplicationsQuery()
            {
                userid = userId
            }, cancellationToken);

            return Ok(applications);
        }


        /// <summary>
        /// Retrieves a specific job application's details by its ID for the authenticated recruiter.
        /// </summary>
        /// <param name="id">The unique identifier of the application.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>The requested application details.</returns>
        [HttpGet("GetApplicationsBYid/{id}")]
        [Authorize(Roles = DS.RECRUITER_ROLE)]
        public async Task<ActionResult<List<ShowAllApplications>>> GetApplicationsById(
            int id,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var applications = await _mediator.Send(new GetApplicationsByIdQuery()
            {
                id = id,
                userid = userId
            }, cancellationToken);

            return Ok(applications);
        }


        /// <summary>
        /// Retrieves all job applications submitted by the authenticated candidate.
        /// </summary>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>A list of the candidate's job applications.</returns>
        [HttpGet("my-applications")]
        [Authorize(Roles = DS.CANDIDATE_ROLE)]
        public async Task<ActionResult<List<ShowMyApp>>> MyApplications(
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var applications = await _mediator.Send(
                new GetMyAppQuery { userid = userId },
                cancellationToken);

            return Ok(applications);
        }


        /// <summary>
        /// Retrieves a specific job application submitted by the authenticated candidate.
        /// </summary>
        /// <param name="id">The unique identifier of the application.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>The requested application details.</returns>
        [HttpGet("my-applicationsBuId/{id}")]
        [Authorize(Roles = DS.CANDIDATE_ROLE)]
        public async Task<ActionResult<List<ShowMyApp>>> MyApplicationsById(
            int id,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var applications = await _mediator.Send(
                new GetMyAppByIdQuery
                {
                    userid = userId,
                    id = id
                },
                cancellationToken);

            return Ok(applications);
        }


        /// <summary>
        /// Creates a new job application for the authenticated user.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to apply for.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>The newly created job application.</returns>
        [HttpPost("Create/{jobId}")]
        [Authorize]
        public async Task<ActionResult<CandidatJopApplication>> Create(
            int jobId,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var application = await _mediator.Send(
                new CreateApplicationCommand
                {
                    userid = userId,
                    jopid = jobId
                },
                cancellationToken);

            return Ok(application);
        }


        /// <summary>
        /// Updates the status of a job application.
        /// </summary>
        /// <param name="id">The unique identifier of the application.</param>
        /// <param name="status">The new status to assign to the application.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>An empty successful response when the status is updated successfully.</returns>
        [HttpPut("status/{id}")]
        [Authorize(Roles = DS.RECRUITER_ROLE)]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromQuery] Applicationstatuse status,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            await _mediator.Send(
                new UpdateStatusCommand
                {
                    userid = userId,
                    appId = id,
                    status = status
                },
                cancellationToken);

            return Ok();
        }


        /// <summary>
        /// Cancels a job application submitted by the authenticated user.
        /// </summary>
        /// <param name="appId">The unique identifier of the application to cancel.</param>
        /// <param name="cancellationToken">Token used to cancel the request if needed.</param>
        /// <returns>No content when the application is successfully canceled.</returns>
        [HttpPut("cancel/{appId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCanceled(
            int appId,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            await _mediator.Send(
                new CancelApplicationCommand
                {
                    userid = userId,
                    Appid = appId
                },
                cancellationToken);

            return NoContent();
        }
    }
}
