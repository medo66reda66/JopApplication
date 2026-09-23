using MediatR;
using System.ComponentModel.DataAnnotations;

namespace JopApplication.Features.Jops.Commands.DeleteJop
{
    public class DeleteJopCommand : IRequest
    {
        [Required]
        public int JopId { get; set; }
    }
}
