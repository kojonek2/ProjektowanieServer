using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public interface IMountainGroupService
    {
        public void CreateMountainGroup(MountainGroup mountainGroup);
        public void EditMountainGroup(MountainGroup mountainGroup);

        public IEnumerable<MountainGroup> GetMountainGroups();
    }
}
