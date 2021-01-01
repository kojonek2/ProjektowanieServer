using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public enum Difficulty
    {
        Easy,
        Moderate,
        Hard,
        Extreme
    }

    [Table("standard_routes_data")]
    public class StandardRouteData
    {
        [JsonIgnore]
        [Key]
        [ForeignKey(nameof(Route))]
        public int RouteId { get; set; }

        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int WalkingTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Difficulty Difficulty { get; set; }


        [JsonIgnore]
        public Route Route { get; set; }
    }
}
