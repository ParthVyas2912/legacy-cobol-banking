using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string FirstName { get; set; } // PIC X(30)

            [MaxLength(30)]
            public string LastName { get; set; } // PIC X(30)

            public Address Address { get; set; }

            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(20)]
            public string City { get; set; } // PIC X(20)

            [MaxLength(2)]
            public string State { get; set; } // PIC X(2)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public enum AccountStatusEnum
        {
            Active, // 88-level: ACTIVE VALUE 'A'
            Inactive, // 88-level: INACTIVE VALUE 'I'
            Suspended // 88-level: SUSPENDED VALUE 'S'
        }

        public class TransactionRecord
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent
        }

        public enum TransactionTypeEnum
        {
            Deposit, // 88-level: DEPOSIT VALUE 'D'
            Withdrawal, // 88-level: WITHDRAWAL VALUE 'W'
            Transfer // 88-level: TRANSFER VALUE 'T'
        }

        public class CustomerTransactionHistory
        {
            public CustomerRecord Customer { get; set; }

            public List<TransactionRecord> Transactions { get; set; } // OCCURS n TIMES equivalent
        }
    }
}