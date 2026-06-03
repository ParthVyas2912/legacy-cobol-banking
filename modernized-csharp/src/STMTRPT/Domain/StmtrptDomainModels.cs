using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class CustomerRecord
        {
            // PIC X(10)
            [MaxLength(10)]
            public string CustomerId { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string CustomerName { get; set; }

            // PIC 9(03) COMP-3
            public decimal RiskScore { get; set; } // PIC 9(03) COMP-3

            // PIC 9(03) COMP-3
            public decimal AccountCount { get; set; } // PIC 9(03) COMP-3

            // PIC X(20)
            [MaxLength(20)]
            public string CustomerStatus { get; set; }

            public Address CustomerAddress { get; set; }
        }

        public class Address
        {
            // PIC X(50)
            [MaxLength(50)]
            public string Street { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string City { get; set; }

            // PIC X(2)
            [MaxLength(2)]
            public string State { get; set; }

            // PIC X(10)
            [MaxLength(10)]
            public string ZipCode { get; set; }
        }

        public class TransactionLedger
        {
            // PIC 9(10)
            public long TransactionId { get; set; } // PIC 9(10)

            // PIC X(10)
            [MaxLength(10)]
            public string AccountNumber { get; set; }

            // PIC 9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            // PIC X(8)
            [MaxLength(8)]
            public string TransactionDate { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string Description { get; set; }
        }

        public class StatementFile
        {
            // PIC X(10)
            [MaxLength(10)]
            public string StatementId { get; set; }

            // PIC X(10)
            [MaxLength(10)]
            public string CustomerId { get; set; }

            // PIC 9(13)V99 COMP-3
            public decimal TotalBalance { get; set; } // PIC 9(13)V99 COMP-3

            // PIC X(8)
            [MaxLength(8)]
            public string StatementDate { get; set; }

            public List<TransactionLedger> Transactions { get; set; } = new List<TransactionLedger>();
        }
    }
}