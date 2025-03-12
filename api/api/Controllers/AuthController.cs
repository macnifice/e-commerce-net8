using api.Business.Interfaces.Auth;
using api.Models.User;
using api.Models.User.Request;
using api.Models.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserRsDto>> Register(RegisterUserRqDto rq)
        {
            RegisterUserRsDto user = await _authService.RegisterAsync(rq);
            if (user is null)
                return BadRequest("User already exists.");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenRsDto>> Login(LoginUserRqDto rq)
        {
            TokenRsDto result = await _authService.LoginAsync(rq);
            if (result is null)
                return BadRequest("Invalid username or password");
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenRsDto>>RefreshToken(RefreshTokenRqDto rq)
        {
            TokenRsDto result = await _authService.RefreshTokenAsync(rq);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid Refresh Token");
            return Ok(result);
        }

        [HttpGet]       
        [Authorize]
        public IActionResult AuthenticatedEndpoint()
        {
            return Ok("You are authenticated");
        }

        [Authorize]
        [HttpGet("user-info/{id}")]
        public async Task<ActionResult<UserViewDto>>UserInfo(int id)
        {
            UserViewDto response = await _authService.UserInfo(id);
            if(response is null)
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Datos inválidos",
                    Detail = "La información proporcionada no es válida"
                });
            return Ok(response);
        }

    }
}
