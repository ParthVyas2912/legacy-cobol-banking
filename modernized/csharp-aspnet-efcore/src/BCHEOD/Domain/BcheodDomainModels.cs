using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class AccountRecord
        {
            [Precision(15, 2)]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(20)]
            public string AccountHolderName { get; set; } // PIC X(20)

            public int AccountNumber { get; set; } // PIC 9(10)

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent

            public TransactionRecord[] Transactions { get; set; } // OCCURS 10 TIMES

            public class TransactionRecord
            {
                [Precision(10, 2)]
                public decimal TransactionAmount { get; set; } // PIC S9(8)V99 COMP-3

                [MaxLength(10)]
                public string TransactionDate { get; set; } // PIC X(10)

                public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent
            }
        }

        public enum AccountStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public enum TransactionTypeEnum
        {
            Deposit, // 88 DEPOSIT VALUE 'D'
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Transfer // 88 TRANSFER VALUE 'T'
        }
    }
}