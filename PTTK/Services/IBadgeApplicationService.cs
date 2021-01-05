using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public interface IBadgeApplicationService
    {
        public IEnumerable<BadgeApplication> GetBadgeApplications(User user);

        public IEnumerable<BadgeApplication> GetApprovedBadgeApplicationsForTurist(int id);

        public void UpdateBadgeApplication(BadgeApplication badgeApplication, int RequesterUserId);
    }
}
