using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        // Root-level domain model for BCHEOD program
        public class Root
        {
            public Header Header { get; set; } = new Header();
            public Detail[] Details { get; set; } = new Detail[10]; // OCCURS 10 TIMES
        }

        public class Header
        {
            [MaxLength(10)]
            public string RecordType { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(20)]
            public string BatchNumber { get; set; } = string.Empty; // PIC X(20)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class Detail
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } = string.Empty; // PIC X(15)

            [Range(-99999999999.99, 99999999999.99)]
            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level equivalent
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit   // 88 DEBIT VALUE 'D'
        }
    }
}