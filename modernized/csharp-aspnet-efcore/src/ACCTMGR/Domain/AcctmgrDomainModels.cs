using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Added for PrecisionAttribute

namespace ACCTMGR.Domain
{
    public class AccountManager
    {
        public class Account
        {
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string AccountName { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // Replaced Precision with Column attribute
            public decimal Balance { get; set; }

            [MaxLength(1)] // PIC X(1)
            public string AccountStatus { get; set; } = string.Empty;

            public AccountStatusEnum StatusEnum
            {
                get => AccountStatus switch
                {
                    "A" => AccountStatusEnum.Active,
                    "I" => AccountStatusEnum.Inactive,
                    "C" => AccountStatusEnum.Closed,
                    _ => AccountStatusEnum.Unknown
                };
                set => AccountStatus = value switch
                {
                    AccountStatusEnum.Active => "A",
                    AccountStatusEnum.Inactive => "I",
                    AccountStatusEnum.Closed => "C",
                    _ => string.Empty
                };
            }

            public enum AccountStatusEnum
            {
                Unknown,
                Active, // VALUE 'A'
                Inactive, // VALUE 'I'
                Closed // VALUE 'C'
            }
        }

        public class Customer
        {
            [MaxLength(15)] // PIC X(15)
            public string CustomerId { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string CustomerName { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string CustomerType { get; set; } = string.Empty;

            public CustomerTypeEnum TypeEnum
            {
                get => CustomerType switch
                {
                    "I" => CustomerTypeEnum.Individual,
                    "C" => CustomerTypeEnum.Corporate,
                    _ => CustomerTypeEnum.Unknown
                };
                set => CustomerType = value switch
                {
                    CustomerTypeEnum.Individual => "I",
                    CustomerTypeEnum.Corporate => "C",
                    _ => string.Empty
                };
            }

            public enum CustomerTypeEnum
            {
                Unknown,
                Individual, // VALUE 'I'
                Corporate // VALUE 'C'
            }
        }

        public class Transaction
        {
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; } = string.Empty;

            [Column(TypeName = "decimal(13,2)")] // Replaced Precision with Column attribute
            public decimal TransactionAmount { get; set; }

            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            public TransactionTypeEnum TypeEnum
            {
                get => TransactionType switch
                {
                    "D" => TransactionTypeEnum.Deposit,
                    "W" => TransactionTypeEnum.Withdrawal,
                    "T" => TransactionTypeEnum.Transfer,
                    _ => TransactionTypeEnum.Unknown
                };
                set => TransactionType = value switch
                {
                    TransactionTypeEnum.Deposit => "D",
                    TransactionTypeEnum.Withdrawal => "W",
                    TransactionTypeEnum.Transfer => "T",
                    _ => string.Empty
                };
            }

            public enum TransactionTypeEnum
            {
                Unknown,
                Deposit, // VALUE 'D'
                Withdrawal, // VALUE 'W'
                Transfer // VALUE 'T'
            }
        }

        public class AccountSummary
        {
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // Replaced Precision with Column attribute
            public decimal TotalCredits { get; set; }

            [Column(TypeName = "decimal(15,2)")] // Replaced Precision with Column attribute
            public decimal TotalDebits { get; set; }

            [Column(TypeName = "decimal(15,2)")] // Replaced Precision with Column attribute
            public decimal NetBalance { get; set; }
        }
    }
}