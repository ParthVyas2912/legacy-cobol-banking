using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            // PIC 9(10) COMP-3
            public decimal TransactionAmount { get; set; } // Precision (10, 0)

            // PIC X(20)
            [MaxLength(20)]
            public string AccountNumber { get; set; }

            // PIC 9(6)
            public int TransactionDate { get; set; }

            // PIC 9(4)
            public int TransactionTime { get; set; }

            // PIC X(2)
            [MaxLength(2)]
            public string TransactionType { get; set; }

            // PIC 9(5)
            public int ATMId { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string Location { get; set; }

            // PIC 9(9)V99 COMP-3
            public decimal BalanceAfterTransaction { get; set; } // Precision (11, 2)

            // PIC X(1)
            [MaxLength(1)]
            public string TransactionStatus { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string ErrorCode { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string ErrorMessage { get; set; }

            // OCCURS 10 TIMES
            public List<AdditionalInfo> AdditionalInfos { get; set; } = new List<AdditionalInfo>(10);

            public class AdditionalInfo
            {
                // PIC X(15)
                [MaxLength(15)]
                public string InfoKey { get; set; }

                // PIC X(50)
                [MaxLength(50)]
                public string InfoValue { get; set; }
            }
        }

        public enum TransactionStatusEnum
        {
            // 88-level values
            Success = 'S', // PIC X(1) VALUE 'S'
            Failure = 'F', // PIC X(1) VALUE 'F'
            Pending = 'P'  // PIC X(1) VALUE 'P'
        }

        public enum TransactionTypeEnum
        {
            // 88-level values
            Withdrawal = 'W', // PIC X(2) VALUE 'W '
            Deposit = 'D',    // PIC X(2) VALUE 'D '
            Inquiry = 'I'     // PIC X(2) VALUE 'I '
        }
    }
}