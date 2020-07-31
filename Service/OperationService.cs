using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public class OperationService
    {
        private BankContext db;
        private ActionsService actions;
        private Operator @operator;

        public OperationService(BankContext db, ActionsService actions, Operator oper)
        {
            this.actions = actions;
            this.db = db;
            this.@operator = oper;
        }

        public TransactionResult MakeTransaction(Account sender, Account receiver, decimal amount, bool force)
        {
            var transactionParameters = db.TransactionCommission
                .FirstOrDefault(arg => arg.SenderTypeId == sender.AccountTypeId && arg.ReceiverTypeId == receiver.AccountTypeId);

            var bankParameters = db.Banks.FirstOrDefault(arg => sender.BankId == arg.Id);

            if (transactionParameters == null || bankParameters == null)
                throw new ApplicationException("Отсутствуют необходимые параметры. Нарушена целостность данныхю");

            var transaction = new Transaction()
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                OperatorId = @operator.Id,
                Date = DateTime.Now,
                Amount = amount,
                BankCommission = amount * (0.01m * (sender.BankId == receiver.BankId ? bankParameters.InnerCommission : bankParameters.OuterCommission)),
                TransactionCommission = amount * (0.01m * transactionParameters.Commission),
                Sender = sender,
                Receiver = receiver
            };

            var totalAmount = transaction.Amount + transaction.BankCommission + transaction.TransactionCommission;

            if (sender.Amount < totalAmount)
                return new TransactionResult()
                {
                    Result = OperationResult.InsufficientFunds,
                    Transaction = null
                };

            var tr = db.Database.BeginTransaction();
            try
            {
                var actStatus = actions.RunActions(transaction, true);
                if (actStatus != OperationResult.Success && !(actStatus == OperationResult.NeedConfirmation && force))
                {
                    tr.Rollback();
                    return new TransactionResult()
                    {
                        Result = actStatus,
                        Transaction = transaction
                    };
                }

                sender.Amount -= totalAmount;
                receiver.Amount += amount;

                db.Update(sender);
                db.Update(receiver);
                db.Add(transaction);

                db.SaveChanges();
                tr.Commit();

                actions.RunActions(transaction, false);
            }
            catch(Exception e)
            {
                tr.Rollback();
                Debug.WriteLine(e.ToString());
                return new TransactionResult()
                {
                    Result = OperationResult.InternalError
                };
            }

            return new TransactionResult()
            {
                Result = OperationResult.Success,
                Transaction = transaction
            };
        }
    }

    public enum OperationResult
    {
        Success, InsufficientFunds, InternalError, NeedConfirmation, IncorrectParameters
    }

    public class TransactionResult
    {
        public OperationResult Result { get; set; }
        public Transaction Transaction { get; set; }
    }


}
