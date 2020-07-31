using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public class SendPaymentDocForeignAction : ITransactionAction
    {
        public OperationResult Execute(Transaction transaction)
        {
            Debug.WriteLine("SendPaymentDocForeign");
            return OperationResult.Success;
        }
    }
}
