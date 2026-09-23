using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Features.Jops.Commands.CloseJop;
using JopApplication.Features.Jops.Commands.CreateJop;
using JopApplication.Features.Jops.Commands.DeleteJop;
using JopApplication.Features.Jops.Commands.UpdateJop;
using JopApplication.Features.Jops.Queries.GetAllJop;
using JopApplication.Features.Jops.Queries.GetAllJopById;
using JopApplication.Features.Jops.Queries.GetAllOpenJops;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DS.RECRUITER_ROLE)]
    public class JopController : ControllerBase
    {

        private readonly IMediator _mediator;
        public JopController( IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all jobs available to the authenticated user.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns a list of jobs if any are found; otherwise, returns NotFound.
        /// </returns>
        [HttpGet("GetAllJops")]
        public async Task<IActionResult> GetAllJops(CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return NotFound();
            }

            var jops = await _mediator.Send(new GetAllJopQuery()
            {
                userid = userid
            }, cancellationToken);

            if (jops == null || !jops.Any())
            {
                return NotFound("No jops found");
            }

            return Ok(jops);
        }


        /// <summary>
        /// Retrieves all open jobs that are currently available.
        /// This endpoint can be accessed without authentication.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns a list of open jobs if any are found; otherwise, returns NotFound.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("GetAllOpenJops")]
        public async Task<IActionResult> GetAllOpenJops(CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return NotFound();
            }

            var jops = await _mediator.Send(new GetAllOpenJopQuery()
            {
                userid = userid
            }, cancellationToken);

            if (jops == null || !jops.Any())
            {
                return NotFound("No open jobs found");
            }

            return Ok(jops);
        }


        /// <summary>
        /// Retrieves a specific job by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the job.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns the requested job if it exists; otherwise, returns NotFound.
        /// </returns>
        [HttpGet("GetJopById/{id}")]
        public async Task<IActionResult> GetJopById(int id, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return NotFound();
            }

            var jop = await _mediator.Send(new GetAllJopByIdQuery()
            {
                Id = id,
                userid = userid
            },cancellationToken);

            if (jop == null)
            {
                return NotFound("Jop not found");
            }

            return Ok(jop);
        }


        /// <summary>
        /// Creates a new job for the authenticated user.
        /// </summary>
        /// <param name="request">
        /// The job information including title, description, location, and image.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns the newly created job with a Created response.
        /// </returns>
        [HttpPost("CreateJop")]
        public async Task<IActionResult> CreateJop(
            [FromQuery] CreateJopRequest request,
            CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userid == null)
            {
                return BadRequest("user not found");
            }

            var jop = await _mediator.Send(new CreateJopCommand()
            {
                Description = request.Description,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                Location = request.Location,
                userid = userid,
            }, cancellationToken);

            return CreatedAtAction(nameof(GetJopById), new { id = jop.Id }, jop);
        }


        /// <summary>
        /// Updates an existing job.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the job to update.
        /// </param>
        /// <param name="request">
        /// The updated job information including title, description, location, and image.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns the updated job after the update is completed.
        /// </returns>
        [HttpPut("EditJop/{id}")]
        public async Task<IActionResult> EditJop(
            int id,
            [FromQuery] UpdateJopRequest request,
            CancellationToken cancellationToken)
        {
            var jop = await _mediator.Send(new UpdateJopCommand()
            {
                Description = request.Description,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                Location = request.Location,
                jopId = id,
            }, cancellationToken);

            return Ok(jop);
        }


        /// <summary>
        /// Deletes an existing job by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the job to delete.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns NoContent when the job is successfully deleted.
        /// </returns>
        [HttpDelete("DeleteJop/{id}")]
        public async Task<IActionResult> DeleteJop(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteJopCommand()
            {
                JopId = id,
            }, cancellationToken);

            return NoContent();
        }


        /// <summary>
        /// Changes the status of a job between open and closed.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the job whose status will be changed.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the request if needed.
        /// </param>
        /// <returns>
        /// Returns NoContent when the job status is successfully changed.
        /// </returns>
        [HttpPut("CloseAndOpenJop/{id}")]
        public async Task<IActionResult> CloseAndOpenJop(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new CloseJopcommand()
            {
                JopId = id,
            }, cancellationToken);

            return NoContent();
        }
    }
}
