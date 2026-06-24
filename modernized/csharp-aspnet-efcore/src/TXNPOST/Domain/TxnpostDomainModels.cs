using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string TransactionId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string AccountNumber { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC S9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(6)] // PIC X(6)
            public string TransactionTime { get; set; } = string.Empty;

            public TransactionStatusEnum TransactionStatus { get; set; }

            public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
        }

        public class TransactionDetail
        {
            [MaxLength(10)] // PIC X(10)
            public string DetailId { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string Description { get; set; } = string.Empty;

            [Column(TypeName = "decimal(10,2)")] // PIC S9(8)V99 COMP-3
            public decimal DetailAmount { get; set; }
        }

        public enum TransactionStatusEnum
        {
            [EnumMember(Value = "P")] // VALUE 'P'
            Pending,

            [EnumMember(Value = "C")] // VALUE 'C'
            Completed,

            [EnumMember(Value = "F")] // VALUE 'F'
            Failed
        }
    }
}