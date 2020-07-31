using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestBank.Models;
using Microsoft.EntityFrameworkCore;
using TestBank.Utils;
using TestBank.Service;

namespace TestBank.Pages
{
    public class AccountsModel : PageModel
    {
        private BankContext db;
        private OperationService operations;

        public AccountsModel(BankContext db, OperationService operations)
        {
            this.db = db;
            this.operations = operations;
        }

        [BindProperty(SupportsGet = true)]
        public int BankId { get; set; }

        [BindProperty]
        public Guid SenderAccountId { get; set; }
        [BindProperty]
        public Guid ReceiverAccountId { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public Transaction Transaction { get; set; }
        public bool NeedConfirmation { get; set; }


        public IEnumerable<Account> Accounts { get; set; }

        public void OnGet()
        {
            if (BankId > 0)
            {
                LoadAccounts();
            }

        }

        private void LoadAccounts()
        {
            Accounts = db.Accounts
                    .Include(k => k.AccountType)
                    .Where(arg => arg.BankId == BankId)
                    .ToArray();
        }

        public void OnPostMakeTransaction(bool force)
        {
            
            try
            {
                var sender = db.Accounts.Include(k => k.Bank).FirstOrDefault(arg => arg.Id == SenderAccountId);
                var receiver = db.Accounts.Include(k => k.Bank).FirstOrDefault(arg => arg.Id == ReceiverAccountId);
                var oper = db.Operators.FirstOrDefault(arg => arg.Login == User.Identity.Name);

                if (Amount <= 0 || sender == null || receiver == null || oper == null)
                {
                    ErrorMessage = "Указаны некорректные параметры";
                    return;
                }

                BankId = sender.BankId;
                LoadAccounts();

                if (SenderAccountId == ReceiverAccountId)
                {
                    ErrorMessage = "Получатель и отправитель - один и тот же счет";
                    return;
                }

                var res = operations.MakeTransaction(sender, receiver, Amount, force);
                switch (res.Result)
                {
                    case OperationResult.Success:
                        SuccessMessage = "Перевод успешно завершен";
                        break;
                    case OperationResult.InsufficientFunds:
                        ErrorMessage = "Недостаточно средств на счёте " + SenderAccountId.ToString();
                        break; ;
                    case OperationResult.InternalError:
                        ErrorMessage = "Внутренняя ошибка сервиса";
                        break;
                    case OperationResult.NeedConfirmation:
                        Transaction = res.Transaction;
                        NeedConfirmation = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            
        }

        public IActionResult OnGetAccountList(int bankId)
        {
            var accounts = db.Accounts.Where(arg => arg.BankId == bankId)
                .Select(arg=>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(arg.Id.ToString(), arg.Id.ToString()));
            return Partial("_SelectOptionsPartial", accounts);
        }
    }
}