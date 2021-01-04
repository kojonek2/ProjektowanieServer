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
    public class BadgeApplicationController : ControllerBase
    {

        private readonly IBadgeApplicationService _badgeApplicationService;

        public BadgeApplicationController(IBadgeApplicationService badgeApplicationService)
        {
            _badgeApplicationService = badgeApplicationService;
        }

        [Authorize]
        [HttpGet("/badgeapplications")]
        public IActionResult Get()
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.isLeader)
            {
                return Unauthorized(new { Message = "User has to be an leader to use this request!" });
            }

            try
            {
                return Ok(_badgeApplicationService.GetBadgeApplications(user));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
