using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class TransactionAction
    {
        public int Id { get; set; }
        public int? SenderAccountTypeId { get; set; }
        public int? ReceiverAccountTypeId { get; set; }
        public int? BankId { get; set; }

        [Required]
        public bool BeforeTransaction { get; set; }
        [Required]
        public string Action { get; set; }

        public Bank Bank { get; set; }
        public AccountType SenderAccountType { get; set; }
        public AccountType ReceiverAccountType { get; set; }
    }
}
