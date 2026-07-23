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

            public decimal TransactionAmount { get; set; } // PIC 9(9)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public AccountDetails Account { get; set; }

            public class AccountDetails
            {
                [MaxLength(10)]
                public string AccountNumber { get; set; } // PIC X(10)

                [MaxLength(20)]
                public string AccountHolderName { get; set; } // PIC X(20)

                public decimal AccountBalance { get; set; } // PIC 9(11)V99 COMP-3
            }

            public enum TransactionTypeEnum
            {
                Withdrawal, // 88-level equivalent value
                Deposit,    // 88-level equivalent value
                Transfer    // 88-level equivalent value
            }
        }
    }
}