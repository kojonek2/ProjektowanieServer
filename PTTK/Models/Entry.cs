using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("entries")]
    public class Entry
    {
        public int Id { get; set; }
        public DateTime DateOfPassing { get; set; }
        public bool Verified { get; set; }

        [ForeignKey(nameof(Route))]
        [JsonIgnore]
        public int RouteId { get; set; }

        [ForeignKey(nameof(Tour))]
        [JsonIgnore]
        public int TourId { get; set; }

        public Route Route { get; set; }
        [JsonIgnore]
        public Tour Tour { get; set; }
    }
}
