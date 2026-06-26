using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRequest
        {
            [MaxLength(10)]
            public string RequestId { get; set; } // PIC X(10)

            public DateTime RequestDate { get; set; } // PIC 9(8)

            [MaxLength(20)]
            public string CustomerName { get; set; } // PIC X(20)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            public Address CustomerAddress { get; set; }

            public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(30)]
            public string City { get; set; } // PIC X(30)

            [MaxLength(2)]
            public string State { get; set; } // PIC X(2)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public class TransactionDetail
        {
            [MaxLength(15)]
            public string TransactionId { get; set; } // PIC X(15)

            public decimal Amount { get; set; } // PIC 9(7)V99 COMP-3

            public TransactionType Type { get; set; }

            public DateTime TransactionDate { get; set; } // PIC 9(8)
        }

        public enum TransactionType
        {
            Purchase, // 88 Purchase VALUE 'P'
            Refund,   // 88 Refund VALUE 'R'
            Transfer  // 88 Transfer VALUE 'T'
        }
    }
}