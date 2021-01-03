using Microsoft.EntityFrameworkCore;
using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public class RoutePointService : IRoutePointService
    {
        private readonly PTTKContext _context;

        public RoutePointService(PTTKContext context)
        {
            _context = context;
        }

        public void CreateRoutePoint(RoutePoint routePoint)
        {
            CheckDuplicateAndRequiredArguments(routePoint);

            _context.RoutePoints.Add(routePoint);
            _context.SaveChanges();
        }

        public void EditRoutePoint(RoutePoint routePoint)
        {
            RoutePoint routePointFromDatabase = _context.RoutePoints.FirstOrDefault(p => p.Id == routePoint.Id);
            if (routePointFromDatabase == null)
            {
                throw new ArgumentException($"Route point with id {routePoint.Id} does not exist!");
            }

            CheckDuplicateAndRequiredArguments(routePoint);

            routePointFromDatabase.Name = routePoint.Name;
            routePointFromDatabase.Description = routePoint.Description;
            routePointFromDatabase.Cordinates = routePoint.Cordinates;
            routePointFromDatabase.Height = routePoint.Height;
            _context.SaveChanges();
        }

        public IEnumerable<RoutePoint> GetRoutePoints()
        {
            return _context.RoutePoints
                .Include(p => p.RoutesStartingWithPoint)
                .Include(p => p.RoutesEndingWithPoint)
                .ToArray();
        }

        private void CheckDuplicateAndRequiredArguments(RoutePoint routePoint)
        {
            if (string.IsNullOrEmpty(routePoint.Name) || string.IsNullOrEmpty(routePoint.Cordinates) || routePoint.Height <= 0)
            {
                throw new ArgumentException("Route point has to contain Name, Cordinates and Height!");
            }

            if (routePoint.Name.Length > RoutePoint.NAME_MAX_LENGTH)
            {
                throw new ArgumentException($"Name has max length is equal to {RoutePoint.NAME_MAX_LENGTH}!");
            }

            if (routePoint.Description != null && routePoint.Description.Length > RoutePoint.DESCRIPTION_MAX_LENGTH)
            {
                throw new ArgumentException($"Description has max length is equal to {RoutePoint.DESCRIPTION_MAX_LENGTH}!");
            }

            if (!Regex.IsMatch(routePoint.Cordinates, RoutePoint.CORDINATES_REGEX))
            {
                throw new ArgumentException($"Cordinates have wrong format!");
            }

            bool duplicate = _context.RoutePoints.Where(p => p.Id != routePoint.Id).Any(p => p.Name == routePoint.Name);
            if (duplicate)
            {
                throw new ArgumentException("Route point with that name already exists!");
            }
        }
    }
}
