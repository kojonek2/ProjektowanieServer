using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Length { get; set; }
        public int SumOfClimbs { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(MountainGroup))]
        public int? MountainGroupId { get; set; }


        public bool isStandardRoute { get { return StandardRouteData != null; } }
        public StandardRouteData? StandardRouteData { get; set; }

        public bool isCustomRoute { get { return CustomRouteData != null; } }
        public CustomRouteData? CustomRouteData { get; set; }

        public MountainGroup? MountainGroup { get; set; }
    }
}
