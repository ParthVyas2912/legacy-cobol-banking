using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } = string.Empty; // PIC X(30)

            public Address CustomerAddress { get; set; } = new Address();

            [Column(TypeName = "decimal(15,2)")]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public StatusEnum Status { get; set; } // 88-level equivalent

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 10 TIMES

            public class Address
            {
                [MaxLength(50)]
                public string Street { get; set; } = string.Empty; // PIC X(50)

                [MaxLength(20)]
                public string City { get; set; } = string.Empty; // PIC X(20)

                [MaxLength(10)]
                public string ZipCode { get; set; } = string.Empty; // PIC X(10)
            }

            public class Transaction
            {
                [MaxLength(15)]
                public string TransactionId { get; set; } = string.Empty; // PIC X(15)

                [Column(TypeName = "decimal(11,2)")]
                public decimal Amount { get; set; } // PIC S9(9)V99 COMP-3

                [MaxLength(8)]
                public string Date { get; set; } = string.Empty; // PIC X(8)
            }

            public enum StatusEnum
            {
                Active = 'A', // VALUE 'A'
                Inactive = 'I', // VALUE 'I'
                Suspended = 'S' // VALUE 'S'
            }
        }
    }
}