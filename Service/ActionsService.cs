using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestBank.Models;

namespace TestBank.Service
{
    public class ActionsService
    {
        BankContext db;
        IServiceProvider services;

        public ActionsService(BankContext db, IServiceProvider services)
        {
            this.services = services;
            this.db = db;
        }

        public OperationResult RunActions(Transaction transaction, bool before)
        {
            var sender = db.Accounts.FirstOrDefault(arg => arg.Id == transaction.SenderId);
            var receiver = db.Accounts.FirstOrDefault(arg => arg.Id == transaction.ReceiverId);

            var bankActions = db.TransactionActions.Where(arg => arg.BeforeTransaction == before)
                .Where(arg => arg.BankId == sender.BankId)
                .ToList();

            foreach (var action in bankActions)
            {
                var res = RunAction(action.Action, transaction);
                if (res != OperationResult.Success)
                    return res;
            }

            var transactionActions = db.TransactionActions.Where(arg => arg.BeforeTransaction == before)
                .Where(arg => arg.SenderAccountTypeId == sender.AccountTypeId)
                .Where(arg => arg.ReceiverAccountTypeId == sender.AccountTypeId)
                .ToList();

            foreach (var action in transactionActions)
            {
                var res = RunAction(action.Action, transaction);
                if (res != OperationResult.Success)
                    return res;
            }

            

            return OperationResult.Success;
        }

        private OperationResult RunAction(string type, Transaction transaction)
        {
            var t = Assembly.GetExecutingAssembly().GetTypes()
                .Where(arg => arg.GetInterface(nameof(ITransactionAction)) != null)
                .Where(arg => arg.Name == type)
                .FirstOrDefault();
                
            if (t == null)
                throw new ApplicationException($"Не найдено указанное действие: {type}");

            var service = services.GetService(t) as ITransactionAction;
            if (service == null)
                throw new ApplicationException($"Не найдено указанное действие: {type}");

            return service.Execute(transaction);
        }
    }
}
