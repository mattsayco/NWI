using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace NWI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(UserRequestDto userRequest)
        {
            var response = await _authService.Register(userRequest);
            if (response.Data is null)
                return NotFound(response);
            if (response.Data == "username error")
                return Conflict(response);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto userRequest)
        {
            var response = await _authService.Login(userRequest);
            var userName = User.Identity?.Name;
            if(userName is null)
                return NotFound();
            var refreshToken = _authService.GenerateRefreshToken(userName);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            if (response.Data is null)
                return NotFound(response);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ServiceResponse<string>>> RefreshToken() {
            var refreshToken = Request.Cookies["refreshToken"];
            var userName = User.Identity?.Name;
            if(userName is null || refreshToken is null)
                return NotFound();
            var response = await _authService.GenerateJwtToken(userName, refreshToken);
            var newRefreshToken = _authService.GenerateRefreshToken(userName);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            return Ok(response);
        }
    }
}