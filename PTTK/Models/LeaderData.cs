using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("leader_data")]
    public class LeaderData
    {
        [JsonIgnore]
        [Key]
        [ForeignKey(nameof(TuristData))]
        public int UserId { get; set; }


        public DateTime NominationDate { get; set; }
        public DateTime? ResignationDate { get; set; }

        [JsonIgnore]
        public TuristData TuristData { get; set; }

        public ICollection<MountainGroup> PermissionsForMountainGroups { get; set; }
    }
}
