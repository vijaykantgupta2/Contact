using Microsoft.AspNetCore.Mvc;
using ContactAPI.Models;
using ContactAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("JwtAuthenticate")]
        [HttpPost]
        [ActionName("JwtAuthenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            var response = _userService.Authenticate(username,password);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

    }
}
