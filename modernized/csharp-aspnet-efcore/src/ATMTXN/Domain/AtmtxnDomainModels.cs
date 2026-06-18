using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmTransaction
    {
        public HeaderRecord Header { get; set; } = new HeaderRecord();
        public TransactionDetail[] TransactionDetails { get; set; } = new TransactionDetail[10]; // OCCURS 10 TIMES
        public TrailerRecord Trailer { get; set; } = new TrailerRecord();

        public class HeaderRecord
        {
            [MaxLength(4)]
            public string RecordType { get; set; } = string.Empty; // PIC X(4)

            [MaxLength(10)]
            public string TerminalId { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(6)]
            public string ProcessingDate { get; set; } = string.Empty; // PIC X(6)
        }

        public class TransactionDetail
        {
            [MaxLength(4)]
            public string RecordType { get; set; } = string.Empty; // PIC X(4)

            [MaxLength(16)]
            public string CardNumber { get; set; } = string.Empty; // PIC X(16)

            [Range(-9999999.99, 9999999.99)]
            public decimal TransactionAmount { get; set; } // PIC S9(7)V99 COMP-3

            [MaxLength(3)]
            public string CurrencyCode { get; set; } = string.Empty; // PIC X(3)

            [MaxLength(6)]
            public string TransactionDate { get; set; } = string.Empty; // PIC X(6)

            [MaxLength(6)]
            public string TransactionTime { get; set; } = string.Empty; // PIC X(6)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public enum TransactionTypeEnum
            {
                Withdrawal, // 88 WITHDRAWAL VALUE 'W'
                Deposit,    // 88 DEPOSIT VALUE 'D'
                Inquiry,    // 88 INQUIRY VALUE 'I'
                Transfer    // 88 TRANSFER VALUE 'T'
            }
        }

        public class TrailerRecord
        {
            [MaxLength(4)]
            public string RecordType { get; set; } = string.Empty; // PIC X(4)

            [Range(0, 99999)]
            public int TotalTransactionCount { get; set; } // PIC 9(5)

            [Range(-99999999999.99, 99999999999.99)]
            public decimal TotalTransactionAmount { get; set; } // PIC S9(11)V99 COMP-3
        }
    }
}