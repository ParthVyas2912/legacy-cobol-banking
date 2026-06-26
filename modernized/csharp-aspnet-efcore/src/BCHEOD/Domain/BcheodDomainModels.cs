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
            public string TransactionId { get; set; } // PIC X(20)

            [RegularExpression(@"\d{8}")]
            public string TransactionDate { get; set; } // PIC 9(8)

            [RegularExpression(@"\d{6}")]
            public string TransactionTime { get; set; } // PIC 9(6)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class Detail
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } // PIC X(15)

            [Range(-99999999999.99, 99999999999.99)]
            public decimal Balance { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level equivalent
        }

        public class Footer
        {
            [Range(0, 99999)]
            public int RecordCount { get; set; } // PIC 9(5)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit // 88 DEBIT VALUE 'D'
        }

        public class BcheodFile
        {
            public Header FileHeader { get; set; }

            public List<Detail> Details { get; set; } // OCCURS n TIMES

            public Footer FileFooter { get; set; }
        }
    }
}