using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal TransactionCommission { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal BankCommission { get; set; }
        [Required]
        public int OperatorId { get; set; }

        public Account Sender { get; set; }
        public Account Receiver { get; set; }
        public Operator Operator { get; set; }
    }
}
