using Microsoft.AspNetCore.Mvc;

namespace Timekeeper.Services.AuthenticationService
{
    [Obsolete]
    [Route("auth")]
    public class AuthController : Controller
    {
        private Authentication _authService;

        public AuthController(Authentication authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            throw new NotImplementedException();
        }
    }
}
