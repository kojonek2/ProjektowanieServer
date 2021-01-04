using Microsoft.EntityFrameworkCore;
using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public class BadgeApplicationService : IBadgeApplicationService
    {
        private readonly PTTKContext _context;

        public BadgeApplicationService(PTTKContext context)
        {
            _context = context;
        }

        public IEnumerable<BadgeApplication> GetBadgeApplications(User user)
        {
            return _context.BadgeApplications.Where(ba => ba.LeaderId == user.Id && ba.Status == VerificationStatus.InProgress)
                .Include(ba => ba.Rank.Badge)
                .Include(ba => ba.Turist)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Tour)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Route.MountainGroup)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Route.StandardRouteData)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Route.CustomRouteData)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Route.StartingPoint)
                .Include(ba => ba.Tours).ThenInclude(t => t.Entries).ThenInclude(e => e.Route.EndingPoint)
                .ToArray();
        }
    }
}
