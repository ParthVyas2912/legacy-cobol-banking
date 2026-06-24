using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class StatementReport
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(30)]
            public string AccountHolderName { get; set; } = string.Empty; // PIC X(30)

            [Column(TypeName = "decimal(15,2)")]
            public decimal AccountBalance { get; set; } = 0.00M; // PIC 9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } = AccountStatusEnum.Active; // 88-level mapping

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS n TIMES

            public class Transaction
            {
                [MaxLength(10)]
                public string TransactionId { get; set; } = string.Empty; // PIC X(10)

                [MaxLength(20)]
                public string Description { get; set; } = string.Empty; // PIC X(20)

                [Column(TypeName = "decimal(13,2)")]
                public decimal Amount { get; set; } = 0.00M; // PIC 9(11)V99 COMP-3

                [MaxLength(8)]
                public string TransactionDate { get; set; } = string.Empty; // PIC X(8)
            }
        }

        public enum AccountStatusEnum
        {
            Active = 'A', // 88 ACCT-ACTIVE VALUE 'A'
            Inactive = 'I', // 88 ACCT-INACTIVE VALUE 'I'
            Closed = 'C' // 88 ACCT-CLOSED VALUE 'C'
        }
    }
}