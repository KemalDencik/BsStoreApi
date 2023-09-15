using Entities.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _service
                .AuthenticationService
                .RegisterUser(userForRegistrationDto);

            if (!result.Succeeded)//başarısızsa
            {
                foreach (var error in result.Errors) 
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
        {
            if (!await _service.AuthenticationService.ValidateUser(userForAuthenticationDto))
                return Unauthorized(); //401

            var tokenDto = await 
                _service
                .AuthenticationService
                .CreateToken(populateExp: true);

            return Ok(tokenDto);
        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))] //dto yu kontrol et
        public async Task<IActionResult> Refresh([FromBody]TokenDto tokenDto)
        {
            //dto dolluysa token dto yu elde etmeye çalış
            var tokenDtoToReturn = await _service
                .AuthenticationService
                .RefreshToken(tokenDto);
            
            return Ok(tokenDtoToReturn);
        }
    }
}
