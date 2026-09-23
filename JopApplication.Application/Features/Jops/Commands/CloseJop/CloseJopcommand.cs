using JopApplication.Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace JopApplication.Features.Jops.Commands.CloseJop
{
    public class CloseJopcommand : IRequest
    {
        [Required]
        public int JopId { get; set; }
    }
}
