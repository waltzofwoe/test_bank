using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class BankCommission
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public decimal InnerCommission { get; set; }
        public decimal OuterCommission { get; set; }

        public Bank Bank { get; set; }
    }
}
