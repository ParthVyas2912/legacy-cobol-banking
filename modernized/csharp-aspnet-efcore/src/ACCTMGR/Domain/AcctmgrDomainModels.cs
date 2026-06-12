using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string AccountName { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string AccountType { get; set; }

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal Balance { get; set; }

            [MaxLength(1)] // PIC X(1)
            public StatusEnum Status { get; set; }

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 10 TIMES

            public class Transaction
            {
                [MaxLength(6)] // PIC X(6)
                public string TransactionCode { get; set; }

                [Precision(11, 2)] // PIC S9(9)V99 COMP-3
                public decimal Amount { get; set; }

                [MaxLength(8)] // PIC X(8)
                public string Date { get; set; }

                [MaxLength(1)] // PIC X(1)
                public string DebitCreditIndicator { get; set; }
            }

            public enum StatusEnum
            {
                Active = 'A', // 88 ACTIVE VALUE 'A'
                Inactive = 'I', // 88 INACTIVE VALUE 'I'
                Closed = 'C' // 88 CLOSED VALUE 'C'
            }
        }
    }
}