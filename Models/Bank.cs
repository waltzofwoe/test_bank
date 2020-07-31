using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class Bank
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal InnerCommission { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal OuterCommission { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
