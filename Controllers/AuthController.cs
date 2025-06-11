using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksManagerAPI.Models.Api;
using TasksManagerAPI.Services;

namespace TasksManagerAPI.Controllers
{
    /*
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseModel>> login(AccesTokenModel request)
        {
            var result = await _jwtService.Autenticate(request);
            if (result is null)
            {
                return Unauthorized();
            }
            return result;
        }
    }*/
}
