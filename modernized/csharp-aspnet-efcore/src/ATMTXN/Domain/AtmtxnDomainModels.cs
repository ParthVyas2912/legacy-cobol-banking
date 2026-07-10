using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public AccountDetails Account { get; set; } // GROUP item

            public List<FeeDetails> Fees { get; set; } = new List<FeeDetails>(); // OCCURS 5 TIMES

            public class AccountDetails
            {
                [MaxLength(10)]
                public string AccountNumber { get; set; } // PIC X(10)

                [MaxLength(30)]
                public string AccountHolderName { get; set; } // PIC X(30)
            }

            public class FeeDetails
            {
                public decimal FeeAmount { get; set; } // PIC 9(5)V99 COMP-3

                [MaxLength(20)]
                public string FeeDescription { get; set; } // PIC X(20)
            }

            public enum TransactionTypeEnum
            {
                Withdrawal, // 88-level equivalent for withdrawal
                Deposit,    // 88-level equivalent for deposit
                Transfer    // 88-level equivalent for transfer
            }
        }
    }
}