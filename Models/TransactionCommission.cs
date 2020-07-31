using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class TransactionCommission
    {
        public int Id { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Commission { get; set; }
        public int SenderTypeId { get; set; }
        public int ReceiverTypeId { get; set; }
        public AccountType SenderType { get; set; }
        public AccountType ReceiverType { get; set; }
    }
}
