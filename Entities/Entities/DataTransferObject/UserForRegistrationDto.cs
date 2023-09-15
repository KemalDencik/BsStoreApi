﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public record UserForRegistrationDto
    {
        public String? FirstName { get; init; }
        public String? LastName { get; init; }

        [Required(ErrorMessage ="UserName is Required")]
        public String? UserName { get; init; }

        [Required(ErrorMessage = "Password is Required")]
        public String? Password { get; init; }

        public String? Email { get; init; }
        public String? PhoneNumber { get; init; }
        public ICollection<String>? Roles{ get; init; }

    }
}
