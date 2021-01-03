using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{

    [Table("route_points")]
    public class RoutePoint
    {
        public const int NAME_MAX_LENGTH = 50;
        public const int DESCRIPTION_MAX_LENGTH = 500;
        public const string CORDINATES_REGEX = "[0-9][0-9]° [0-9][0-9]' [0-9][0-9]\" [NS] [0-9][0-9]° [0-9][0-9]' [0-9][0-9]\" [EW]";

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Cordinates { get; set; }
        public int Height { get; set; }

        //Route with the same name is the same route but in different direction
        public int UsedInRoutes { get { return RoutesStartingWithPoint.Concat(RoutesEndingWithPoint).Select(r => r.Name).Distinct().Count(); } }

        [JsonIgnore]
        public ICollection<Route> RoutesStartingWithPoint { get; set; }

        [JsonIgnore]
        public ICollection<Route> RoutesEndingWithPoint { get; set; }
    }
}
