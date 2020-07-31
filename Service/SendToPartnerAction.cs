using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public class SendToPartnerAction : ITransactionAction
    {
        public OperationResult Execute(Transaction transaction)
        {
            Debug.WriteLine("SendToPArtner");
            return OperationResult.Success;
        }
    }
}
