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
    [Authorize(Roles = DS.RECRUITER_ROLE)]
    public class JopController : ControllerBase
    {
        private readonly IJopService _jopService;
        public JopController(IJopService jopService)
        {
            _jopService = jopService;
        }
        [HttpGet("GetAllJops")]
        public async Task<IActionResult> GetAllJops(CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) 
                {
                  return NotFound();
                }
            var jops = await _jopService.GetAllmYJops(userid , cancellationToken);
            if (jops == null || !jops.Any())
            {
                return NotFound("No jops found");
            }
            return Ok(jops);
        }

        [AllowAnonymous]
        [HttpGet("GetAllOpenJops")]
        public async Task<IActionResult> GetAllOpenJops(CancellationToken cancellationToken)
        {
            var jops = await _jopService.GetAllOpenJops(cancellationToken);
            if (jops == null || !jops.Any())
            {
                return NotFound("No open jobs found");
            }
            return Ok(jops);
        }
        [HttpGet("GetJopById/{id}")]
        public async Task<IActionResult> GetJopById(int id, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return NotFound();
            }

            var jop = await _jopService.GetJopById(id,userid, cancellationToken);
            if (jop == null)
            {
                return NotFound("Jop not found");
            }
            return Ok(jop);
        }
        [HttpPost("CreateJop")]
        public async Task<IActionResult> CreateJop([FromQuery] CreateJopRequest request, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return BadRequest("user not found");
            }

            var jop = await _jopService.createJop(request, cancellationToken,userid);
            return CreatedAtAction(nameof(GetJopById), new { id = jop.Id }, jop);
        }
        [HttpPut("EditJop/{id}")]
        public async Task<IActionResult> EditJop(int id, [FromQuery] UpdateJopRequest request, CancellationToken cancellationToken)
        {
            var jop = await _jopService.Edit(id, request, cancellationToken);
            return Ok(jop);
        }
        [HttpDelete("DeleteJop/{id}")]
        public async Task<IActionResult> DeleteJop(int id, CancellationToken cancellationToken)
        {
            await _jopService.Delete(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("CloseAndOpenJop/{id}")]
        public async Task<IActionResult> CloseAndOpenJop(int id,CancellationToken cancellationToken)
        {
           await _jopService.CloseAndOpenJop(id,cancellationToken);
            return NoContent();
        }

    }
}
