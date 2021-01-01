using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTTK.Models;
using PTTK.Services;
using PTTK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Controllers
{
    [ApiController]
    public class MountainGroupController : Controller
    {
        private IMountainGroupService _mountainGroupService;

        public MountainGroupController(IMountainGroupService mountainGroupService)
        {
            _mountainGroupService = mountainGroupService;
            
        }

        [Authorize]
        [HttpPost("/mountaingroup/create")]
        public IActionResult Create(MountainGroup mountainGroup)
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                _mountainGroupService.CreateMountainGroup(mountainGroup);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("/mountaingroup/edit")]
        public IActionResult Edit(MountainGroup mountainGroup)
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                _mountainGroupService.EditMountainGroup(mountainGroup);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("/mountaingroups")]
        public IActionResult Get()
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                return Ok(_mountainGroupService.GetMountainGroups());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
