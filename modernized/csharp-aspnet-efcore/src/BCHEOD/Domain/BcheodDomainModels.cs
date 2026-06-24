using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Added for PrecisionAttribute

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class RootRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string RecordId { get; set; } = string.Empty;

            public Header Header { get; set; } = new Header();

            public List<Detail> Details { get; set; } = new List<Detail>();

            public Trailer Trailer { get; set; } = new Trailer();
        }

        public class Header
        {
            [MaxLength(8)] // PIC X(8)
            public string FileDate { get; set; } = string.Empty;

            [MaxLength(6)] // PIC X(6)
            public string FileTime { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string FileName { get; set; } = string.Empty;
        }

        public class Detail
        {
            [MaxLength(15)] // PIC X(15)
            public string AccountNumber { get; set; } = string.Empty;

            [Column(TypeName = "decimal(13, 2)")] // Replacing Precision with Column attribute
            public decimal TransactionAmount { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string CurrencyCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        }

        public class Trailer
        {
            [MaxLength(8)] // PIC X(8)
            public string RecordCount { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15, 2)")] // Replacing Precision with Column attribute
            public decimal TotalAmount { get; set; }
        }

        public enum TransactionStatus
        {
            Pending, // Default
            Completed,
            Failed
        }
    }
}