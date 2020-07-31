using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public int AccountTypeId { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public Bank Bank { get; set; }
        public AccountType AccountType { get; set; }
    }
}
