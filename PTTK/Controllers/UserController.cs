using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTTK.Models;
using PTTK.Services;
using PTTK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest request)
        {
            AuthenticateResponse response = _userService.Authenticate(request);

            if (response == null)
                return BadRequest(new { message = "Bad login or password!" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            return Ok(user);
        }
    }
}
