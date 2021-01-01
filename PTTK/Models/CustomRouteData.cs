using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("custom_routes_data")]
    public class CustomRouteData
    {
        [JsonIgnore]
        [Key]
        [ForeignKey(nameof(Route))]
        public int RouteId { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public Route Route { get; set; }
    }
}
