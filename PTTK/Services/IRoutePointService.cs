using PTTK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Services
{
    public interface IRoutePointService
    {
        public void CreateRoutePoint(RoutePoint routePoint);
        public void EditRoutePoint(RoutePoint routePoint);

        public IEnumerable<RoutePoint> GetRoutePoints();
    }
}
