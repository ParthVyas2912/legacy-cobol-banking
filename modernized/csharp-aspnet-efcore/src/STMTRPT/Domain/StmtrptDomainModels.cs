using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class AccountSummary
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountHolderName { get; set; } // PIC X(30)

            public decimal CurrentBalance { get; set; } // PIC S9(13)V99 COMP-3

            public decimal AvailableBalance { get; set; } // PIC S9(13)V99 COMP-3

            public Transaction[] Transactions { get; set; } // OCCURS 100 TIMES
        }

        public class Transaction
        {
            [MaxLength(10)]
            public string TransactionDate { get; set; } // PIC X(10)

            [MaxLength(15)]
            public string TransactionDescription { get; set; } // PIC X(15)

            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level equivalent
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit   // 88 DEBIT VALUE 'D'
        }

        public class CustomerDetails
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } // PIC X(30)

            [MaxLength(50)]
            public string Address { get; set; } // PIC X(50)

            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            public AccountSummary[] Accounts { get; set; } // OCCURS 10 TIMES
        }
    }
}