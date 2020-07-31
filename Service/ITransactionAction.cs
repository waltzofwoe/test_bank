using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public interface ITransactionAction
    {
        OperationResult Execute(Transaction transaction);
    }
}
