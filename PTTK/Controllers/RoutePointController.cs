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
    public class RoutePointController : ControllerBase
    {
        private IRoutePointService _routePointService;

        public RoutePointController(IRoutePointService routePointService)
        {
            _routePointService = routePointService;

        }

        [Authorize]
        [HttpPost("/routepoint/create")]
        public IActionResult Create(RoutePoint routePoint)
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                _routePointService.CreateRoutePoint(routePoint);
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
        [HttpPost("/routepoint/edit")]
        public IActionResult Edit(RoutePoint routePoint)
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                _routePointService.EditRoutePoint(routePoint);
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
        [HttpGet("/routepoints")]
        public IActionResult Get()
        {
            User user = HttpContext.Items[Constants.UserKey] as User;
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized(new { Message = "User has to be an admin to use this request!" });
            }

            try
            {
                return Ok(_routePointService.GetRoutePoints());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
