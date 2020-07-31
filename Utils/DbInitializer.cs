using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using TestBank.Models;
using TestBank.Service;

namespace TestBank.Utils
{
    public class DbInitializer
    {
        private BankContext db;
        private AuthorizationService auth;


        public DbInitializer(BankContext db, AuthorizationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public void Init()
        {
            const string personType = "Физическое лицо";
            const string legalType = "Юридическое лицо";
            const string notResType = "Нерезидент";

            if (db.AccountTypes.Any() || db.TransactionCommission.Any() || db.Banks.Any())
                return;

            auth.Register("Operator", "Gfhjkm123");

            var person = new AccountType() { Name = personType };
            var legal = new AccountType() { Name = legalType };
            var notres = new AccountType() { Name = notResType };

            db.AccountTypes.AddRange(new[] { person, legal, notres });
            db.SaveChanges();

            db.TransactionCommission.AddRange(new[]
            {
                new TransactionCommission() { SenderType = person, ReceiverType = person, Commission=0 },
                new TransactionCommission() { SenderType = legal, ReceiverType = person, Commission=0 },
                new TransactionCommission() { SenderType = notres, ReceiverType = person, Commission=0 },
                new TransactionCommission() { SenderType = person, ReceiverType = legal, Commission=2},
                new TransactionCommission() { SenderType = legal, ReceiverType = legal, Commission=3},
                new TransactionCommission() { SenderType = notres, ReceiverType = legal, Commission=4 },
                new TransactionCommission() { SenderType = person, ReceiverType = notres, Commission=4 },
                new TransactionCommission() { SenderType = legal, ReceiverType = notres, Commission=6 },
                new TransactionCommission() { SenderType = notres, ReceiverType = notres, Commission=6 },
            });

            db.TransactionActions.AddRange(new[]
            {
                new TransactionAction() { SenderAccountType = person, ReceiverAccountType= person, BeforeTransaction=false, Action = "SmsAction" },
                new TransactionAction() { SenderAccountType = legal, ReceiverAccountType= person, BeforeTransaction=false, Action = "SmsAction" },
                new TransactionAction() { SenderAccountType = notres, ReceiverAccountType= person, BeforeTransaction=false, Action = "SmsAction" },
                new TransactionAction() { SenderAccountType = person, ReceiverAccountType= legal, BeforeTransaction=false, Action = "SendPaymentDocAction" },
                new TransactionAction() { SenderAccountType = legal, ReceiverAccountType= legal, BeforeTransaction=false, Action = "SendPaymentDocAction" },
                new TransactionAction() { SenderAccountType = notres, ReceiverAccountType= legal, BeforeTransaction=false, Action = "SendPaymentDocAction" },
                new TransactionAction() { SenderAccountType = person, ReceiverAccountType= notres, BeforeTransaction=false, Action = "SendPaymentDocForeignAction" },
                new TransactionAction() { SenderAccountType = legal, ReceiverAccountType= notres, BeforeTransaction=false, Action = "SendPaymentDocForeignAction" },
                new TransactionAction() { SenderAccountType = notres, ReceiverAccountType= notres, BeforeTransaction=false, Action = "SendPaymentDocForeignAction" },
            });


            db.SaveChanges();

            var bank1 = new Bank()
            {
                Name = "Сбербанк",
                Accounts = new[]
                    {
                        new Account()
                        {
                            AccountType = legal,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = person,
                            Amount = 1000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = notres,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        }
                    },
                InnerCommission = 0,
                OuterCommission = 1
            };

            var bank2 = new Bank()
            {
                Name = "ВТБ",
                Accounts = new[]
                    {
                        new Account()
                        {
                            AccountType = legal,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = person,
                            Amount = 1000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = notres,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        }
                    },
                InnerCommission = 0,
                OuterCommission = 2
            };

            var bank3 = new Bank()
            {
                Name = "Альфабанк",
                Accounts = new[]
                    {
                        new Account()
                        {
                            AccountType = legal,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = person,
                            Amount = 1000,
                            Id = Guid.NewGuid()
                        },
                        new Account()
                        {
                            AccountType = notres,
                            Amount = 100000,
                            Id = Guid.NewGuid()
                        }
                    },
                InnerCommission = 1,
                OuterCommission = 2.5m,
            };

            db.AddRange(bank1, bank2, bank3);
            db.SaveChanges();


            db.Add(new TransactionAction() { BankId = bank1.Id, BeforeTransaction = false, Action = "SendToInspectionAction" });
            db.Add(new TransactionAction() { BankId = bank2.Id, BeforeTransaction = false, Action = "SendToPartnerAction" });
            db.Add(new TransactionAction() { BankId = bank3.Id, BeforeTransaction = true, Action = "ShowConfirmWindowAction" });
            db.SaveChanges();
        }
    }
}
