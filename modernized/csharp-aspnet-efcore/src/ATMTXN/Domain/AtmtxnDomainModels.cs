using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level equivalent

            public AccountDetails AccountDetails { get; set; }

            public TerminalDetails TerminalDetails { get; set; }
        }

        public class AccountDetails
        {
            [MaxLength(16)]
            public string AccountNumber { get; set; } // PIC X(16)

            [MaxLength(4)]
            public string AccountType { get; set; } // PIC X(4)

            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class TerminalDetails
        {
            [MaxLength(8)]
            public string TerminalId { get; set; } // PIC X(8)

            [MaxLength(20)]
            public string Location { get; set; } // PIC X(20)
        }

        public enum TransactionType
        {
            Withdrawal, // 88-level value
            Deposit,    // 88-level value
            Transfer    // 88-level value
        }
    }
}