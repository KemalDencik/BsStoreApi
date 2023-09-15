using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public record UserForAuthenticationDto
    {
        [Required (ErrorMessage = "username is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "password is required")]
        public string? Password { get; init; }
    }
}
