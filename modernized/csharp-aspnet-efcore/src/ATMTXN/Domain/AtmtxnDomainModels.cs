using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Added for PrecisionAttribute

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } = string.Empty; // PIC X(12)

            [MaxLength(16)]
            public string AccountNumber { get; set; } = string.Empty; // PIC X(16)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } = 0m; // PIC S9(13)V99 COMP-3

            [MaxLength(1)]
            public string TransactionType { get; set; } = string.Empty; // PIC X(1)

            public TransactionStatus Status { get; set; } = TransactionStatus.Pending; // 88-level equivalent

            public DateTime TransactionDate { get; set; } = DateTime.MinValue; // PIC 9(8) for YYYYMMDD

            public TransactionLocation Location { get; set; } = new TransactionLocation();

            public List<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();
        }

        public class TransactionLocation
        {
            [MaxLength(20)]
            public string City { get; set; } = string.Empty; // PIC X(20)

            [MaxLength(2)]
            public string State { get; set; } = string.Empty; // PIC X(2)

            [MaxLength(10)]
            public string ZipCode { get; set; } = string.Empty; // PIC X(10)
        }

        public class TransactionDetail
        {
            [MaxLength(10)]
            public string ItemCode { get; set; } = string.Empty; // PIC X(10)

            [Precision(9, 2)]
            public decimal ItemAmount { get; set; } = 0m; // PIC S9(7)V99 COMP-3

            [MaxLength(30)]
            public string Description { get; set; } = string.Empty; // PIC X(30)
        }

        public enum TransactionStatus
        {
            Pending, // Equivalent to 88-level value for "Pending"
            Completed, // Equivalent to 88-level value for "Completed"
            Failed // Equivalent to 88-level value for "Failed"
        }
    }
}