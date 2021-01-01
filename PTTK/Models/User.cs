using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsAdmin { get; set; }

        public bool isTurist { get { return TuristData != null; } }
        public TuristData? TuristData { get; set; }

        public bool isLeader { get { return isTurist && TuristData.IsLeader; } }
    }
}
