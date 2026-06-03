using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        /// <summary>
        /// Represents the transaction record structure.
        /// </summary>
        public class TransactionRecord
        {
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(8)]
            public string TransactionDate { get; set; } // PIC X(8)

            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            [MaxLength(50)]
            public string Description { get; set; } // PIC X(50)
        }

        /// <summary>
        /// Represents the transaction type as an enumeration.
        /// </summary>
        public enum TransactionTypeEnum
        {
            Credit, // 88 CREDIT VALUE "C".
            Debit   // 88 DEBIT VALUE "D".
        }

        /// <summary>
        /// Represents the working storage fields.
        /// </summary>
        public class WorkingStorage
        {
            [Range(0, 999999999)]
            public decimal RecordCount { get; set; } // PIC 9(09) COMP-3

            [Range(0, 999999999)]
            public decimal FailCount { get; set; } // PIC 9(09) COMP-3
        }

        /// <summary>
        /// Represents the audit output structure.
        /// </summary>
        public class AuditOutput
        {
            [MaxLength(10)]
            public string AuditId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal AuditAmount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(8)]
            public string AuditDate { get; set; } // PIC X(8)

            [MaxLength(6)]
            public string AuditTime { get; set; } // PIC X(6)

            [MaxLength(50)]
            public string AuditDescription { get; set; } // PIC X(50)
        }

        /// <summary>
        /// Represents the transaction input file structure.
        /// </summary>
        public class TransactionInputFile
        {
            public TransactionRecord[] Transactions { get; set; } // OCCURS n TIMES
        }
    }
}