using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBank.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> opt) : base(opt) { }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<TransactionCommission> TransactionCommission { get; set; }
        public DbSet<TransactionAction> TransactionActions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Transaction>()
                .HasOne(arg => arg.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(arg => arg.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(arg => arg.Operator)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionCommission>()
               .HasOne(arg => arg.SenderType)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionCommission>()
                .HasOne(arg => arg.ReceiverType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionAction>()
           .HasOne(arg => arg.SenderAccountType)
           .WithMany()
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionAction>()
                .HasOne(arg => arg.ReceiverAccountType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
