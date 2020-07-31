using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class Operator
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public byte[] Password { get; set; }
    }
}
