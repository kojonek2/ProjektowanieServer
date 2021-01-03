using Microsoft.EntityFrameworkCore;
using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public class MountainGroupService : IMountainGroupService
    {
        private readonly PTTKContext _context;

        public MountainGroupService(PTTKContext context)
        {
            _context = context;
        }

        public void CreateMountainGroup(MountainGroup mountainGroup)
        {
            CheckDuplicateAndRequiredArguments(mountainGroup);

            _context.MountainGroups.Add(mountainGroup);
            _context.SaveChanges();
        }

        public void EditMountainGroup(MountainGroup mountainGroup)
        {
            MountainGroup mountainGroupFromDatabase = _context.MountainGroups.FirstOrDefault(m => m.Id == mountainGroup.Id);
            if (mountainGroupFromDatabase == null)
            {
                throw new ArgumentException($"Mountain group with id {mountainGroup.Id} does not exist!");
            }

            CheckDuplicateAndRequiredArguments(mountainGroup);

            mountainGroupFromDatabase.Name = mountainGroup.Name;
            mountainGroupFromDatabase.Abbreviation = mountainGroup.Abbreviation;
            _context.SaveChanges();
        }

        public IEnumerable<MountainGroup> GetMountainGroups()
        {
            return _context.MountainGroups
                .Include(m => m.LeadersWithPermisions)
                .Include(m => m.RoutesInGroup)
                .ToArray();
        }

        private void CheckDuplicateAndRequiredArguments(MountainGroup mountainGroup)
        {
            if (string.IsNullOrEmpty(mountainGroup.Name) || string.IsNullOrEmpty(mountainGroup.Abbreviation))
            {
                throw new ArgumentException("Mountain group has to contain Name and Abbreviation!");
            }

            if (mountainGroup.Name.Length > MountainGroup.NAME_MAX_LENGTH)
            {
                throw new ArgumentException($"Name has max length is equal to {MountainGroup.NAME_MAX_LENGTH}!");
            }

            if (mountainGroup.Abbreviation.Length > MountainGroup.ABBREVIATION_MAX_LENGTH)
            {
                throw new ArgumentException($"Abbreviation has max length is equal to {MountainGroup.ABBREVIATION_MAX_LENGTH}!");
            }

            bool duplicate = _context.MountainGroups.Where(m => m.Id != mountainGroup.Id).Any(m => m.Name == mountainGroup.Name);
            if (duplicate)
            {
                throw new ArgumentException("Mountain group with that name already exists!");
            }
        }
    }
}
