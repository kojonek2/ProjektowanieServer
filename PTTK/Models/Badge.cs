using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public enum BadgeType
    {
        IntoMountains,
        Popular,
        Small,
        Big,
        ForPersistance
    }

    [Table("badges")]
    public class Badge
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BadgeType Type { get; set; }

        [JsonIgnore]
        public ICollection<BadgeRank> Ranks { get; set; }
    }
}
