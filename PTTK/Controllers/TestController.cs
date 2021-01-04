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
        public IEnumerable<User> Tourists()
        {
            return _context.Users.Include(u => u.TuristData).ThenInclude(t => t.Tours).Where(u => u.TuristData != null).ToArray();
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
            var a = _context.Routes
                .Include(r => r.CustomRouteData)
                .Include(r => r.StandardRouteData)
                .Include(r => r.MountainGroup)
                .Include(r => r.StartingPoint)
                .Include(r => r.EndingPoint)
                .Include(r => r.Entries)
                .ToArray();
            return a;
        }

        [Route("routepoints1")]
        public IEnumerable<RoutePoint> RoutePoints()
        {
            return _context.RoutePoints
                .Include(p => p.RoutesStartingWithPoint)
                .Include(p => p.RoutesEndingWithPoint)
                .ToArray();
        }

        [Route("badgeranks1")]
        public IEnumerable<BadgeRank> BadgeRanks()
        {
            return _context.BadgeRanks
                .Include(r => r.Badge)
                .ToArray();
        }

        [Route("tours1")]
        public IEnumerable<Tour> Tours()
        {
            return _context.Tours
                .Include(t => t.Entries).ThenInclude(e => e.Route.MountainGroup)
                .Include(t => t.Entries).ThenInclude(e => e.Route.StandardRouteData)
                .Include(t => t.Entries).ThenInclude(e => e.Route.CustomRouteData)
                .Include(t => t.Entries).ThenInclude(e => e.Route.StartingPoint)
                .Include(t => t.Entries).ThenInclude(e => e.Route.EndingPoint)
                .Include(t => t.Turist)
                .ToArray();
        }


        [Route("badgeapplications1")]
        public IEnumerable<BadgeApplication> BadgeApplications()
        {
            return _context.BadgeApplications
                .Include(t => t.Rank.Badge)
                .Include(t => t.Turist.User)
                .Include(t => t.Leader.TuristData.User)
                .Include(t => t.Tours).ThenInclude(t => t.Entries)
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
