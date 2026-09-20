using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        private readonly ICandidateJopAppService _candidateJopAppService;

        public AppController(ICandidateJopAppService candidateJopAppService)
        {
            _candidateJopAppService = candidateJopAppService;
        }

        [HttpGet("GetAllApplications")]
        [Authorize(Roles =DS.RECRUITER_ROLE)]
        public async Task<ActionResult<List<ShowAllApplications>>> GetAllApplications(
          CancellationToken cancellationToken)
        {
            var applications =
                await _candidateJopAppService.GetAllApplications(cancellationToken);

            return Ok(applications);
        }

        // Get My Applications
        [HttpGet("my-applications")]
        [Authorize(Roles = DS.CANDIDATE_ROLE)]
        public async Task<ActionResult<List<ShowMyApp>>> MyApplications(
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var applications =
                await _candidateJopAppService.MyApp(userId, cancellationToken);

            return Ok(applications);
        }

        // Create Application
        [HttpPost("Create/{jobId}")]
        [Authorize]
        public async Task<ActionResult<CandidatJopApplication>> Create(
            int jobId,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var application =
                await _candidateJopAppService.Create(
                    userId,
                    jobId,
                    cancellationToken);

            return Ok(application);
        }


        [HttpPut("status/{id}")]
        [Authorize(Roles = DS.RECRUITER_ROLE)]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromQuery]Applicationstatuse status,
            CancellationToken cancellationToken)
        {
            await _candidateJopAppService.UpdateStatus(id, status, cancellationToken);

            return Ok();
        }

        [HttpPut("cancel/{appId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCanceled(
              int appId,
              CancellationToken cancellationToken)
        {
            await _candidateJopAppService.updateCanceled(
                appId,
                cancellationToken);

            return NoContent();
        }
    }
}
