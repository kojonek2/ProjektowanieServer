using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("badge_applications")]
    public class BadgeApplication
    {
        public const int DESCRIPTION_MAX_LENGTH = 500;

        public int Id { get; set; }
        public DateTime? AwardDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VerificationStatus Status { get; set; }
        public string? Description { get; set; }


        [ForeignKey(nameof(Rank))]
        [JsonIgnore]
        public int RankId { get; set; }


        [ForeignKey(nameof(Leader))]
        public int LeaderId { get; set; }


        [ForeignKey(nameof(Turist))]
        public int TuristId { get; set; }

        public BadgeRank Rank { get; set; }

        [JsonIgnore]
        public LeaderData Leader { get; set; }
        public TuristData Turist { get; set; }

        public ICollection<Tour> Tours { get; set; }
    }
}
