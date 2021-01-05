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

        public IEnumerable<BadgeApplication> GetApprovedBadgeApplicationsForTurist(int id)
        {
            return _context.BadgeApplications
                .Include(r => r.Rank.Badge)
                .Where(ba => ba.TuristId == id && ba.Status == VerificationStatus.Approved)
                .ToArray();
        }

        public void UpdateBadgeApplication(BadgeApplication badgeApplication, int RequesterUserId)
        {
            BadgeApplication badgeApplicationFromDatabase = _context.BadgeApplications.FirstOrDefault(ba => ba.Id == badgeApplication.Id);
            if (badgeApplicationFromDatabase == null)
            {
                throw new ArgumentException($"Badge application with id {badgeApplication.Id} does not exist!");
            }

            if (badgeApplicationFromDatabase.LeaderId != RequesterUserId)
            {
                throw new UnauthorizedAccessException("This request can be performed only by assigned leader!");
            }

            if (badgeApplication.Description.Length > BadgeApplication.DESCRIPTION_MAX_LENGTH)
            {
                throw new ArgumentException($"Description length must be lower or equal to {BadgeApplication.DESCRIPTION_MAX_LENGTH}");
            }

            if (badgeApplication.Status == VerificationStatus.Rejected && badgeApplication.Description == null)
            {
                throw new ArgumentException($"Application has to contain description when it is being rejected!");
            }

            if (badgeApplication.Status == VerificationStatus.Approved)
                badgeApplicationFromDatabase.AwardDate = DateTime.Now.Date;

            badgeApplicationFromDatabase.Description = badgeApplication.Description;
            badgeApplicationFromDatabase.Status = badgeApplication.Status;
            _context.SaveChanges();
        }
    }
}
