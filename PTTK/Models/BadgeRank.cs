using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("badge_ranks")]
    public class BadgeRank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Quota { get; set; }

        [ForeignKey(nameof(Badge))]
        [JsonIgnore]
        public int BadgeId { get; set; }

        public Badge Badge { get; set; }

        [JsonIgnore]
        public ICollection<BadgeApplication> BadgeApplications { get; set; }
    }
}
