using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public class ShowConfirmWindowAction : ITransactionAction
    {
        public OperationResult Execute(Transaction transaction)
        {
            return OperationResult.NeedConfirmation;
        }
    }
}
