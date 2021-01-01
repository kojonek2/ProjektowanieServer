using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("turists_data")]
    public class TuristData
    {
        [JsonIgnore]
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        

        public string FirstName { get; set; }
        public string SureName { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public bool IsLeader { get { return LeaderData != null && (LeaderData.ResignationDate == null || LeaderData.ResignationDate >= DateTime.Now); } }
        public LeaderData? LeaderData { get; set; }
    }
}
