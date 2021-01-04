using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }

        [ForeignKey(nameof(Turist))]
        public int TuristId { get; set; }

        [JsonIgnore]
        public TuristData Turist { get; set; }

        public ICollection<Entry> Entries { get; set; }

        [JsonIgnore]
        public ICollection<BadgeApplication> BadgeApplications { get; set; }
    }
}
