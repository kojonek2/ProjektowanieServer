using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("mountain_group")]
    public class MountainGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }


        public int? LeadersWithPermissionsCount { get { return LeadersWithPermisions?.Count; } }

        public int? RoutesInGroupCount { get { return RoutesInGroup?.Count; } }

        [JsonIgnore]
        public ICollection<LeaderData> LeadersWithPermisions { get; set; }

        [JsonIgnore]
        public ICollection<Route> RoutesInGroup { get; set; }
    }
}
