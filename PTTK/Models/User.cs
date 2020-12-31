using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsAdmin { get; set; }

        public bool isTurist { get { return TuristData != null; } }
        public TuristData? TuristData { get; set; }
       
    }
}
