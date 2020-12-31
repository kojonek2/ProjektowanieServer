using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Models
{
    [Table("turists_data")]
    public class TuristData
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        

        public string FirstName { get; set; }
        public string SureName { get; set; }

        public User User { get; set; }
    }
}
