using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTTK.Models;
using PTTK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly PTTKContext _context;

        public TestController(PTTKContext context)
        {
            _context = context;
        }

        [Authorize]
        [Route("logins")]
        public IEnumerable<string> GetLogins()
        {
            return _context.Users.Select(u => u.Login).ToArray();
        }

        [Route("turists")]
        public IEnumerable<bool> Tourists()
        {
            return _context.Users.Include(u => u.TuristData).Select(u => u.isTurist).ToArray();
        }

        [Route("leaders")]
        public IEnumerable<bool> Leaders()
        {
            return _context.Users.Include(u => u.TuristData).ThenInclude(td => td.LeaderData).Select(u => u.isLeader).ToArray();
        }

        [Route("leaders1")]
        public IEnumerable<User> Leaders1()
        {
            return _context.Users.Include(u => u.TuristData)
                .ThenInclude(td => td.LeaderData)
                .ThenInclude(l => l.PermissionsForMountainGroups)
                .Where(u => u.TuristData != null && u.TuristData.LeaderData != null)
                .ToArray();
        }

        [Route("routes")]
        public IEnumerable<Route> Routes()
        {
            return _context.Routes
                .Include(r => r.CustomRouteData)
                .Include(r => r.StandardRouteData)
                .Include(r => r.MountainGroup)
                .ToArray();
        }

        [Route("hash/{password}")]
        public string GetHash(string password)
        {
            return PasswordValidator.createHash(password);
        }

        [Route("validate/{login}/{password}")]
        public bool GetHash(string login, string password)
        {
            User user = _context.Users.First(u => u.Login == login);

            return PasswordValidator.isValid(password, user.Password);
        }
    }
}
