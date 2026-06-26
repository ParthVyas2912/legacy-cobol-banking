using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRequest
        {
            [MaxLength(10)] // PIC X(10)
            public string RequestId { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string CustomerName { get; set; }

            [MaxLength(15)] // PIC X(15)
            public string AccountNumber { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string CurrencyCode { get; set; }

            [Range(0, 9999999999.99)] // PIC 9(10)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            [DataType(DataType.Date)] // PIC 9(8)
            public DateTime TransactionDate { get; set; }

            public Address CustomerAddress { get; set; }

            public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
        }

        public class Address
        {
            [MaxLength(50)] // PIC X(50)
            public string Street { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string City { get; set; }

            [MaxLength(2)] // PIC X(2)
            public string State { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string PostalCode { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string CountryCode { get; set; }
        }

        public class TransactionDetail
        {
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; }

            [Range(0, 99999999.99)] // PIC 9(8)V99 COMP-3
            public decimal Amount { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string Currency { get; set; }

            public TransactionType Type { get; set; }

            [DataType(DataType.Date)] // PIC 9(8)
            public DateTime Date { get; set; }
        }

        public enum TransactionType
        {
            Credit, // 88-level value
            Debit   // 88-level value
        }
    }
}