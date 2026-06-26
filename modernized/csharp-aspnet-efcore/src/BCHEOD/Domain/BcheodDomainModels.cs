using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Header
        {
            [MaxLength(10)]
            public string RecordType { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string BatchNumber { get; set; } // PIC X(20)

            public DateTime ProcessingDate { get; set; } // PIC 9(8)

            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class Detail
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } // PIC X(15)

            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public enum TransactionTypeEnum
            {
                Credit, // 88 CREDIT VALUE 'C'
                Debit   // 88 DEBIT VALUE 'D'
            }
        }

        public class Trailer
        {
            public int RecordCount { get; set; } // PIC 9(5)

            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class BcheodFile
        {
            public Header FileHeader { get; set; }

            public List<Detail> Details { get; set; } // OCCURS n TIMES

            public Trailer FileTrailer { get; set; }
        }
    }
}