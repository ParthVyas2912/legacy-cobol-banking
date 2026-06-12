using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class AccountDetails
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent
        }

        public enum AccountStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public class TransactionDetails
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent
        }

        public enum TransactionTypeEnum
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit // 88 DEBIT VALUE 'D'
        }

        public class CustomerDetails
        {
            [MaxLength(15)]
            public string CustomerId { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string CustomerName { get; set; } // PIC X(50)

            [MaxLength(100)]
            public string Address { get; set; } // PIC X(100)

            public List<AccountDetails> Accounts { get; set; } // OCCURS n TIMES equivalent
        }
    }
}