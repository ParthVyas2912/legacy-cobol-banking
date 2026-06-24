using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Added for PrecisionAttribute

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class BcheodRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string RecordId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string Description { get; set; } = string.Empty;

            [Precision(9, 2)] // PIC 9(7)V99 COMP-3
            public decimal Amount { get; set; }

            [DataType(DataType.Date)] // PIC 9(8)
            public DateTime TransactionDate { get; set; }

            public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

            public class TransactionDetail
            {
                [MaxLength(5)] // PIC X(5)
                public string DetailCode { get; set; } = string.Empty;

                [Precision(7, 2)] // PIC 9(5)V99 COMP-3
                public decimal DetailAmount { get; set; }

                [MaxLength(15)] // PIC X(15)
                public string DetailDescription { get; set; } = string.Empty;
            }
        }

        public enum RecordStatus
        {
            Active = 'A', // 88 ACTIVE VALUE 'A'
            Inactive = 'I' // 88 INACTIVE VALUE 'I'
        }
    }
}