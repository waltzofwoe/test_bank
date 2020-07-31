using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestBank.Models;

namespace TestBank.Service
{
    public class AuthorizationService
    {
        private BankContext db;

        public AuthorizationService(BankContext db)
        {
            this.db = db;
        }

        public bool Login(string login, string password)
        {
            var oper = db.Operators.FirstOrDefault(arg => arg.Login == login);
            return oper?.Password.SequenceEqual(GetPasswordHash(password)) == true;
        }

        public void Register(string login, string password)
        {
            var oper = db.Operators.FirstOrDefault(arg => arg.Login == login);
            if (oper != null)
                throw new ApplicationException("Логин занят");

            if (password == null)
                throw new ApplicationException("Не указан пароль");

            oper = new Operator()
            {
                Login = login,
                Password = GetPasswordHash(password)
            };

            db.Add(oper);
            db.SaveChanges();
        }

        private unsafe byte[] GetPasswordHash(string str)
        {
            str += "12856sdsfkh@#%!";

            var bytes = Encoding.UTF8.GetBytes(str);

            var md5 = MD5.Create();

            var hash = md5.ComputeHash(bytes);

            return hash;
        }
    }
}
